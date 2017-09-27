using BackEndSAM.DataAcces.ArmadoBD;
using BackEndSAM.Models.Armado;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ArmadoController : ApiController
    {

        //obtenemos la spools a partir de la orden de trabajo.
        public object Get(string ordenTrabajo, int tipo, string token, string lenguaje)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                IdOrdenTrabajo idOrdenTrabajo = new IdOrdenTrabajo();

                List<Sam3_Steelgo_Get_SpoolID_Result> lista = (List<Sam3_Steelgo_Get_SpoolID_Result>)ArmadoBD.Instance.ObtenerIDOrdenTrabajo(usuario, ordenTrabajo, tipo, lenguaje);
                List<IDS> listaAtatus = new List<IDS>();
                if (lista.Count > 0)
                {
                    listaAtatus.Add(new IDS());
                    foreach (var item in lista)
                    {
                        listaAtatus.Add(new IDS { Status = item.status, IDValido = item.ID, Proyecto = item.NombreProyecto, Valor = item.OrdenTrabajoSpoolID, ProyectoID = item.ProyectoID, HabilitadoHoldFecha = item.HabilitadoHoldFecha });
                    }

                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = lista[0].OrdenTrabajo,
                        idStatus = listaAtatus
                    };
                }
                else
                {
                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = "",
                        idStatus = listaAtatus
                    };
                };
                return idOrdenTrabajo;

            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }

        }

        //obtenemos los campos predeterminados
        [HttpGet]
        public object ObtieneCamposPredeterminados(string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                IdOrdenTrabajo idOrdenTrabajo = new IdOrdenTrabajo();

                CamposPredeterminados armadoCamposPredeterminados = new CamposPredeterminados();

                armadoCamposPredeterminados = new CamposPredeterminados
                {
                    FechaArmado = (string)ArmadoBD.Instance.ObtenerValorFecha(usuario, lenguaje, 4),
                    Muestra = (string)ArmadoBD.Instance.ObtenerValorFecha(usuario, lenguaje, 6),
                    Llena = (string)ArmadoBD.Instance.ObtenerValorFecha(usuario, lenguaje, 12),
                    TipoCaptura = (string)ArmadoBD.Instance.ObtenerValorFecha(usuario, lenguaje, 17),
                    FormatoFecha = lenguaje == "es-MX" ? "dd/MM/yyyy" : "MM/dd/yyyy"
                };

                return armadoCamposPredeterminados;
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        //obtenemos el detalle del grid.
        public object Get(string JsonCaptura, bool isReporte, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DetalleCaptura capturaDatosJson = serializer.Deserialize<DetalleCaptura>(JsonCaptura);
                capturaDatosJson.SinCaptura = capturaDatosJson.SinCaptura == "Todos" ? "1" : "0";
                List<DetalleDatosJson> listaDetalleDatos = new List<DetalleDatosJson>();
                //List<JuntaSpool> listaJuntasXSpool = null;


                List<Sam3_Armado_Get_Detalle_Result> detalle = (List<Sam3_Armado_Get_Detalle_Result>)ArmadoBD.Instance.ObtenerDetalleArmado(capturaDatosJson, usuario, lenguaje);

                List<TrabajosAdicionalesXJunta> listaTrabajoAdicionalXJunta = (List<TrabajosAdicionalesXJunta>)ArmadoBD.Instance.listaTrabajosAdicionalesXJunta(usuario);
                foreach (Sam3_Armado_Get_Detalle_Result item in detalle)
                {
                    List<Sam3_Armado_Get_MaterialesSpool_Result> listaNumeroUnicos = (List<Sam3_Armado_Get_MaterialesSpool_Result>)ArmadoBD.Instance.listaNumeroUnicos(item.JuntaSpoolID, usuario, 2, capturaDatosJson.SinCaptura);
                    List<NumeroUnico> listNumeroUnico1 = GenerarListaNumerosUnicos(listaNumeroUnicos, 1, detalle[0] != null ? detalle[0].LongitudMaterial1 : 0);
                    List<NumeroUnico> listNumeroUnico2 = GenerarListaNumerosUnicos(listaNumeroUnicos, 2, detalle[0] != null ? detalle[0].LongitudMaterial2 : 0);

                    //item.TipoJunta
                    DetalleDatosJson detalleDatos = new DetalleDatosJson
                    {

                        AccionNumeroUnico = (item.Clave1 != null || item.Clave2 != null  || item.NumeroUnico1ID.ToString() != "" || item.NumeroUnico2ID.ToString() != "") ? 2 : 1,
                        Accion = item.JuntaSpoolIDArmado == null ? 1 : 2,
                        IDProyecto = item.ProyectoID,
                        IdOrdenTrabajo = item.OrdenTrabajoID,
                        OrdenTrabajo = item.OrdenTrabajo,
                        IdVal = item.OrdenTrabajoSpoolID,
                        SpoolID = item.NumeroControl,
                        JuntaID = item.JuntaSpoolID,
                        Junta = item.Etiqueta,
                        TipoJunta = item.TipoJunta,
                        Diametro = item.Diametro.ToString().Replace(',', '.'),
                        Cedula = item.Cedula,
                        FechaArmado = item.FechaArmado,
                        TipoJuntaID = item.TipoJuntaID,
                        TuberoID = item.Tubero == null ? "" : item.ObreroID.ToString(),
                        Tubero = item.Tubero == null ? "" : item.Tubero,
                        TallerID = item.TallerID == null ? "" : item.TallerID.ToString(),
                        Taller = item.Taller == null ? "" : item.Taller,
                        Localizacion = item.Localizacion,
                        FamiliaAcero = item.FamiliaAcero,
                        NumeroUnico1 = (item.NumeroUnico1ID == null || item.NumeroUnico1ID == 0) ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].Clave : "") : item.Clave1.ToString(),
                        NumeroUnico2 = (item.NumeroUnico2ID == null || item.NumeroUnico2ID == 0) ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].Clave : "") : item.Clave2.ToString(),
                        TemplateMensajeTrabajosAdicionales = item.TabajosAdicionales,
                        ListaNumerosUnicos1 = listNumeroUnico1,
                        ListaNumerosUnicos2 = listNumeroUnico2,
                        ListaTaller = ObtenerListaTaller((List<Sam3_SteelGo_Get_Taller_Result>)ArmadoBD.Instance.ObtenerTallerXPoryecto(usuario, item.ProyectoID.GetValueOrDefault())),
                        ListaTubero = ObtenerListaTubero((List<Sam3_Steelgo_Get_Obrero_Result>)ArmadoBD.Instance.ObtenerTuberoXProyecto(usuario, item.ProyectoID.GetValueOrDefault(), 2)),
                        // ListaDetalleTrabajoAdicional = listDetalleTrabajoAdicional,
                        listadoTrabajosAdicionalesXJunta = listaTrabajoAdicionalXJunta,
                        SinCaptura = capturaDatosJson.SinCaptura,
                        NumeroUnico1ID = item.NumeroUnico1ID == null ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].NumeroUnicoID.ToString() : "") : item.NumeroUnico1ID.ToString(),
                        NumeroUnico2ID = item.NumeroUnico1ID == null ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].NumeroUnicoID.ToString() : "") : item.NumeroUnico2ID.ToString(),
                        DetalleJunta = "Junta: " + item.TipoJunta + " - " + "Ced: " + item.Cedula + " - " + "Loc: " + item.Localizacion + " - " + "Acero: " + item.FamiliaAcero + "",
                        //RowOk = true,
                        LongitudMaterial1 = item.LongitudMaterial1,
                        LongitudMaterial2 = item.LongitudMaterial2,
                        RevisionNU = item.RevisionNU
                    };

                    listaDetalleDatos.Add(detalleDatos);
                }
                return serializer.Serialize(listaDetalleDatos.OrderByDescending(x => x.Junta));
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        private List<Tubero> ObtenerListaTubero(List<Sam3_Steelgo_Get_Obrero_Result> listaTubero)
        {
            List<Tubero> listaTuberos = new List<Tubero>();

            listaTuberos.Add(new Tubero
            {
                Codigo = "",
                ObreroID = 0,
                TipoObrero = ""
            });
            foreach (Sam3_Steelgo_Get_Obrero_Result item in listaTubero)
            {
                Tubero tubero = new Tubero
                {
                    ObreroID = item.ObreroID,
                    Codigo = item.Codigo
                };
                listaTuberos.Add(tubero);
            }
            return listaTuberos;
        }

        private List<Taller> ObtenerListaTaller(List<Sam3_SteelGo_Get_Taller_Result> listaTaller)
        {
            List<Taller> listaTalleres = new List<Taller>();
            foreach (Sam3_SteelGo_Get_Taller_Result item in listaTaller)
            {
                Taller taller = new Taller
                {
                    TallerID = item.TallerID,
                    Nombre = item.Nombre
                };
                listaTalleres.Add(taller);
            }
            return listaTalleres;
        }

        public List<NumeroUnico> GenerarListaNumerosUnicos(List<Sam3_Armado_Get_MaterialesSpool_Result> listaNumerosUnicos, int numeroSeleccionado, int? longitudMaterial)
        {
            List<NumeroUnico> numerosUnicos = new List<NumeroUnico>();
            numerosUnicos.Add(new NumeroUnico());
            foreach (Sam3_Armado_Get_MaterialesSpool_Result item in listaNumerosUnicos)
            {
                if (int.Parse(item.Etiqueta.ToString()) == numeroSeleccionado && item.LongitudMaterial <= longitudMaterial)
                {
                    NumeroUnico numeroUnico = new NumeroUnico
                    {
                        NumeroUnicoID = item.NumeroUnicoID,
                        Clave = item.Clave,
                        EtiquetaMaterial = int.Parse(item.EtiquetaMaterial.ToString()),
                        Etiqueta = item.Etiqueta,
                        JuntasEncontradas = item.JuntasEntocontradas,
                        LongitudMaterial = item.LongitudMaterial,
                        ItemCodeID = item.ItemCodeID,
                        Nombre = item.Nombre,
                        TipoMaterialID = item.TipoMaterialID

                    };
                    numerosUnicos.Add(numeroUnico);
                }
            }
            return numerosUnicos;
        }

        public List<DetalleTrabajoAdicional> GenerarDetalleAdicionalJson(List<Sam3_Armado_Get_DetalleTrabajoAdicional_Result> listaTrabajoAdicional, Sam3_Usuario usuario)
        {
            List<DetalleTrabajoAdicional> listaDetalleAdicional = new List<DetalleTrabajoAdicional>();
            foreach (Sam3_Armado_Get_DetalleTrabajoAdicional_Result item in listaTrabajoAdicional)
            {
                DetalleTrabajoAdicional detalleAdicional = new DetalleTrabajoAdicional
                {
                    Accion = item.JuntaSpoolID == 0 ? 1 : 2,
                    Observacion = item.Observacion,
                    TrabajoAdicional = item.TrabajoAdicional,
                    TrabajoAdicionalID = item.TrabajoAdicionalID,
                    ArmadoTrabajoAdicionalID = item.ArmadoTrabajoAdicionalID

                };
                listaDetalleAdicional.Add(detalleAdicional);
            }
            return listaDetalleAdicional;
        }

        //obtenemos las juntas.
        public object Get(string ordenTrabajo, string id, string sinCaptura, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ArmadoBD.Instance.ObtenerJuntasXSpoolID(usuario, ordenTrabajo, id, sinCaptura == "Todos" ? 1 : 0);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
        //obtenemos los tuberos
        public object Get(int idProyecto, int tipo, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ObtenerListaTubero((List<Sam3_Steelgo_Get_Obrero_Result>)ArmadoBD.Instance.ObtenerTuberoXProyecto(usuario, idProyecto, tipo));
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
        //obtenemos los talleres
        public object Get(int idProyecto, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ArmadoBD.Instance.ObtenerTallerXPoryecto(usuario, idProyecto);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Post(Captura listaCapturaArmado, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                DataTable TabajosAdicionales = null;
                DataTable TablaNumerosUnicosAsignados = null;
                foreach (DetalleGuardarJson item in listaCapturaArmado.Detalles)
                {
                    if (item.ListaDetalleTrabajoAdicional != null)
                    {
                        foreach (DetalleGuardarTrabajoAdicional detalleTrabajoAdicional in item.ListaDetalleTrabajoAdicional)
                        {
                            detalleTrabajoAdicional.Accion = detalleTrabajoAdicional.Accion == 0 ? 1 : detalleTrabajoAdicional.Accion;
                            detalleTrabajoAdicional.JuntaID = detalleTrabajoAdicional.JuntaID == null ? item.JuntaID : detalleTrabajoAdicional.JuntaID;

                        }
                        if (TabajosAdicionales == null)
                            TabajosAdicionales = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaDetalleTrabajoAdicional);
                        else
                            TabajosAdicionales.Merge(Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaDetalleTrabajoAdicional));
                    }
                    if (TablaNumerosUnicosAsignados == null)
                        TablaNumerosUnicosAsignados = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaNumeroUnicoAsignado);
                    else
                        TablaNumerosUnicosAsignados.Merge(Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaNumeroUnicoAsignado));

                }
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaArmado.Detalles);
                dtDetalleCaptura.Columns.Remove("ListaDetalleTrabajoAdicional");
                dtDetalleCaptura.Columns.Remove("ListaNumeroUnicoAsignado");
                return ArmadoBD.Instance.InsertarCapturaArmado(dtDetalleCaptura, TabajosAdicionales, TablaNumerosUnicosAsignados, usuario, lenguaje);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Get(string token, int juntaID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<Sam3_Armado_Get_DetalleTrabajoAdicional_Result> detallaArmadoAdicional = (List<Sam3_Armado_Get_DetalleTrabajoAdicional_Result>)ArmadoBD.Instance.DetallaArmadoAdicional(juntaID, usuario);
                return GenerarDetalleAdicionalJson(detallaArmadoAdicional, usuario);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Put(ElementosCapturados listaCapturaActualizar, string token, string lenguaje, string SinCaptura)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleActualizaCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaActualizar.Detalles);
                List<DetalleDatosJson> listaDetalleDatos = new List<DetalleDatosJson>();
                DataTable detalle = (DataTable)ArmadoBD.Instance.ActualizaDatos(dtDetalleActualizaCaptura, lenguaje);

                List<TrabajosAdicionalesXJunta> listaTrabajoAdicionalXJunta = (List<TrabajosAdicionalesXJunta>)ArmadoBD.Instance.listaTrabajosAdicionalesXJunta(usuario);

                foreach (DataRow item in detalle.Rows)
                {
                    List<Sam3_Armado_Get_MaterialesSpool_Result> listaNumeroUnicos = (List<Sam3_Armado_Get_MaterialesSpool_Result>)ArmadoBD.Instance.listaNumeroUnicos(int.Parse(item["JuntaSpoolID"].ToString()), usuario, 2, SinCaptura == "Todos" ? "1" : "0");
                    List<NumeroUnico> listNumeroUnico1 = GenerarListaNumerosUnicos(listaNumeroUnicos, 1, int.Parse(item["LongitudMaterial1"].ToString()));
                    List<NumeroUnico> listNumeroUnico2 = GenerarListaNumerosUnicos(listaNumeroUnicos, 2, int.Parse(item["LongitudMaterial2"].ToString()));
    

                    //item.TipoJunta
                    DetalleDatosJson detalleDatos = new DetalleDatosJson
                    {

                        Accion = item["JuntaSpoolIDArmado"].ToString() == "" ? 1 : 2,
                        AccionNumeroUnico = (item["NumeroUnico1ID"].ToString() != "" || item["NumeroUnico1ID"].ToString() != "" || item["Clave1"].ToString() != "" || item["Clave2"].ToString() != "") ? 2 : 1,
                        IDProyecto = int.Parse(item["ProyectoID"].ToString()),
                        IdOrdenTrabajo = int.Parse(item["OrdenTrabajoID"].ToString()),
                        OrdenTrabajo = item["OrdenTrabajo"].ToString(),
                        IdVal = int.Parse(item["OrdenTrabajoSpoolID"].ToString()),
                        SpoolID = item["NumeroControl"].ToString(),
                        JuntaID = int.Parse(item["JuntaSpoolID"].ToString()),
                        Junta = item["Etiqueta"].ToString(),
                        TipoJunta = item["TipoJunta"].ToString(),
                        Diametro = item["Diametro"].ToString().Replace(',', '.'),
                        Cedula = item["Cedula"].ToString(),
                        FechaArmado = item["FechaArmado"].ToString() == "" ? null : item["FechaArmado"].ToString(),
                        TipoJuntaID = int.Parse(item["TipoJuntaID"].ToString()),
                        TuberoID = item["TuberoID"].ToString(),
                        Tubero = item["Tubero"].ToString(),
                        TallerID = item["TallerID"].ToString() == null ? "" : item["TallerID"].ToString(),
                        Taller = item["Taller"].ToString() == null ? "" : item["Taller"].ToString(),
                        ListaTaller = ObtenerListaTaller((List<Sam3_SteelGo_Get_Taller_Result>)ArmadoBD.Instance.ObtenerTallerXPoryecto(usuario, int.Parse(item["ProyectoID"].ToString()))),
                        ListaTubero = ObtenerListaTubero((List<Sam3_Steelgo_Get_Obrero_Result>)ArmadoBD.Instance.ObtenerTuberoXProyecto(usuario, int.Parse(item["ProyectoID"].ToString()), 2)),
                        Localizacion = item["Localizacion"].ToString(),
                        FamiliaAcero = item["FamiliaAcero"].ToString(),
                        NumeroUnico1 = (item["NumeroUnico1ID"].ToString() == null || item["NumeroUnico1ID"].ToString() == "") ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].Clave : "") : item["Clave1"].ToString(),
                        NumeroUnico2 = (item["NumeroUnico2ID"].ToString() == null || item["NumeroUnico2ID"].ToString() == "") ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].Clave : "") : item["Clave2"].ToString(),
                        TemplateMensajeTrabajosAdicionales = item["TabajosAdicionales"].ToString(),
                        ListaNumerosUnicos1 = listNumeroUnico1,
                        ListaNumerosUnicos2 = listNumeroUnico2,
                        listadoTrabajosAdicionalesXJunta = listaTrabajoAdicionalXJunta,
                        SinCaptura = SinCaptura,
                        NumeroUnico1ID = item["NumeroUnico1ID"].ToString() == "" ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].NumeroUnicoID.ToString() : "") : item["NumeroUnico1ID"].ToString(),
                        NumeroUnico2ID = item["NumeroUnico1ID"].ToString() == "" ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].NumeroUnicoID.ToString() : "") : item["NumeroUnico2ID"].ToString(),
                        DetalleJunta = "Junta: " + item["TipoJunta"].ToString() + " - " + "Ced: " + item["Cedula"].ToString() + " - " + "Loc: " + item["Localizacion"].ToString() + " - " + "Acero: " + item["FamiliaAcero"].ToString() + "",
                        //RowOk = true,
                        LongitudMaterial1 = int.Parse(item["LongitudMaterial1"].ToString()),
                        LongitudMaterial2 = int.Parse(item["LongitudMaterial2"].ToString()),
                        RevisionNU = bool.Parse(item["RevisionNU"].ToString())
                    };
                    listaDetalleDatos.Add(detalleDatos);
                }
                return serializer.Serialize(listaDetalleDatos.OrderByDescending(x => x.Junta));
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Put(ValidarCaptura Captura, string token)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtValidarNumerosUnicos = null;
                if (Captura != null)
                    dtValidarNumerosUnicos = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(Captura.Detalles);

                List<string> listaSpoolsIncorrectos = (List<string>)ArmadoBD.Instance.ValidarNumerosUnicos(dtValidarNumerosUnicos);
                return listaSpoolsIncorrectos;
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }


    }
}
