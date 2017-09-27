using BackEndSAM.Controllers.Inspeccion.Dimensional;
using BackEndSAM.DataAcces;
using BackEndSAM.DataAcces.ArmadoBD;
using BackEndSAM.DataAcces.Inspeccion.VisualDimensionalBD;
using BackEndSAM.DataAcces.Sam3General;
using BackEndSAM.DataAcces.Sam3General.CapturasRapidas;
using BackEndSAM.DataAcces.Sam3General.Defectos;
using BackEndSAM.DataAcces.Sam3General.TipoResultado;
using BackEndSAM.Models.Inspeccion.Dimensional;
using BackEndSAM.Models.Inspeccion.VisualDimensional;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Inspeccion.VisualDimensional
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisualDimensionalController : ApiController
    {

        public object Get(string JsonCaptura, string token, string Lenguaje, string juntasSeleccionadas)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                CapturaVisualDimensional capturaDatosJson = serializer.Deserialize<CapturaVisualDimensional>(JsonCaptura);

                List<CapturaVisualDimensional> listaDetalleDatos = new List<CapturaVisualDimensional>();

                List<Sam3_InspeccionVisual_Get_Detalle_Result> detalle = (List<Sam3_InspeccionVisual_Get_Detalle_Result>)VisualDimensionalBD.Instance.ObtenerDetalleJunta(capturaDatosJson, usuario, Lenguaje, juntasSeleccionadas);
                string fecha = (string)ArmadoBD.Instance.ObtenerValorFecha(usuario, Lenguaje, 16);

                foreach (Sam3_InspeccionVisual_Get_Detalle_Result item in detalle)
                {
                    List<Sam3_Armado_Get_MaterialesSpool_Result> listaNumeroUnicos = (List<Sam3_Armado_Get_MaterialesSpool_Result>)VisualDimensionalBD.Instance.listaNumeroUnicos(item.JuntaSpoolID, usuario, 1);
                    List<NumeroUnico> listNumeroUnico1 = GenerarListaNumerosUnicos(listaNumeroUnicos, 1);
                    List<NumeroUnico> listNumeroUnico2 = GenerarListaNumerosUnicos(listaNumeroUnicos, 2);

                    CapturaVisualDimensional detalleDatos = new CapturaVisualDimensional
                    {
                        Accion = item.JuntaSpoolIDVisual == null ? 1 : 2,
                        AccionNumeroUnico = (item.Clave1 != null || item.Clave2 != null || item.NumeroUnico1ID.ToString() != "" || item.NumeroUnico2ID.ToString() != "") ? 2 : 1,
                        Proyecto = capturaDatosJson.Proyecto,
                        OrdenTrabajoID = capturaDatosJson.OrdenTrabajoID,
                        OrdenTrabajo = capturaDatosJson.OrdenTrabajo,
                        OrdenTrabajoSpoolID = capturaDatosJson.OrdenTrabajoSpoolID,
                        OrdenTrabajoSpool = capturaDatosJson.OrdenTrabajoSpool,
                        SpoolID = capturaDatosJson.OrdenTrabajo + "-" + capturaDatosJson.OrdenTrabajoSpool,
                        JuntaID = item.JuntaSpoolID,
                        Junta = item.Etiqueta,
                        TipoJunta = item.TipoJunta,
                        TipoJuntaID = item.TipoJuntaID.ToString(),
                        Diametro = item.Diametro,
                        FechaInspeccion = item.FechaInspeccion,
                        Defectos = (item.Defecto == null ? "" : item.Defecto.ToString()),
                        DefectosID = (item.DefectoID == null ? "0" : item.DefectoID.ToString()),
                        TallerID = item.TallerID == null ? capturaDatosJson.TallerID : item.TallerID.ToString(),
                        Taller = item.Taller == null ? capturaDatosJson.Taller : item.Taller,

                        Inspector = item.Inspector,
                        InspectorID = item.ObreroID.ToString(),

                        NumeroUnico1 = (item.NumeroUnico1ID == null || item.NumeroUnico1ID == 0) ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].Clave : "") : item.Clave1.ToString(),
                        NumeroUnico2 = (item.NumeroUnico2ID == null || item.NumeroUnico2ID == 0) ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].Clave : "") : item.Clave2.ToString(),
                        NumeroUnico1ID = (item.NumeroUnico1ID == null || item.NumeroUnico1ID == 0) ? (listNumeroUnico1.Count == 2 ? listNumeroUnico1[1].NumeroUnicoID.ToString() : "") : item.NumeroUnico1ID.ToString(),
                        NumeroUnico2ID = (item.NumeroUnico2ID == null || item.NumeroUnico2ID == 0) ? (listNumeroUnico2.Count == 2 ? listNumeroUnico2[1].NumeroUnicoID.ToString() : "") : item.NumeroUnico2ID.ToString(),
                        ListaNumerosUnicos1 = listNumeroUnico1,
                        ListaNumerosUnicos2 = listNumeroUnico2,
                        ProyectoID = capturaDatosJson.ProyectoID,
                        Resultado = item.Resultado == null ? capturaDatosJson.Resultado : item.Resultado.ToString(),
                        ResultadoID = item.ResultadoID == null ? capturaDatosJson.ResultadoID : item.ResultadoID.ToString(),

                        ListaInspector = ObtenerListaInspector((List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(capturaDatosJson.ProyectoID, 2, "Inspector Visual Dimensional")),
                        ListaTaller = ObtenerListaTaller((List<Sam3_SteelGo_Get_Taller_Result>)TallerBD.Instance.ObtenerTallerXPoryecto(capturaDatosJson.ProyectoID)),
                        ListaDefectos = ObtenerListaDefectos((List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(Lenguaje, "Inspección Visual")),
                        ListaResultados = ObtenerListaResultado((List<Sam3_Steelgo_Get_TipoResultado_Result>)TipoResultadoBd.Instance.ObtenerListadoResultados(Lenguaje)),

                        DetalleJunta = "Junta: " + item.TipoJunta + " - " + "Ced: " + item.Cedula + " - " + "Loc: " + item.Localizacion + " - " + "Acero: " + item.FamiliaAcero + "",

                        //EtiquetaMaterial1 = item.EtiquetaMaterial1,
                        //EtiquetaMaterial2 = item.EtiquetaMaterial2,
                        
                        Localizacion = item.Localizacion,
                        RevisionNU = item.RevisionNU
                    };

                    listaDetalleDatos.Add(detalleDatos);
                }
                return serializer.Serialize(listaDetalleDatos.OrderBy(x => x.Junta));
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
        public List<NumeroUnico> GenerarListaNumerosUnicos(List<Sam3_Armado_Get_MaterialesSpool_Result> listaNumerosUnicos, int numeroSeleccionado)
        {
            List<NumeroUnico> numerosUnicos = new List<NumeroUnico>();

            numerosUnicos.Add(new NumeroUnico
            {
                NumeroUnicoID = 0,
                Clave = "",
                EtiquetaMaterial = 0,
                Etiqueta = ""
            });

            foreach (Sam3_Armado_Get_MaterialesSpool_Result item in listaNumerosUnicos)
            {
                if (int.Parse(item.Etiqueta.ToString()) == numeroSeleccionado)
                {
                    NumeroUnico numeroUnico = new NumeroUnico
                    {

                        NumeroUnicoID = item.NumeroUnicoID,
                        Clave = item.Clave,
                        EtiquetaMaterial = int.Parse(item.EtiquetaMaterial.ToString()),
                        Etiqueta = item.Etiqueta,
                        JuntasEncontradas = item.JuntasEntocontradas,
                        ItemCodeID = item.ItemCodeID,
                        LongitudMaterial = item.LongitudMaterial,
                        Nombre = item.Nombre,
                        TipoMaterialID = item.TipoMaterialID

                    };
                    numerosUnicos.Add(numeroUnico);
                }

            }

            return numerosUnicos;
        }
        private List<Inspector> ObtenerListaInspector(List<Sam3_Steelgo_Get_Obrero_Result> listaInspector)
        {
            List<Inspector> listaInspectors = new List<Inspector>();

            listaInspectors.Add(new Inspector
            {
                ObreroID = 0,
                Codigo = "",
                NombreCompleto = ""
            });

            foreach (Sam3_Steelgo_Get_Obrero_Result item in listaInspector)
            {
                Inspector tubero = new Inspector
                {
                    ObreroID = item.ObreroID,
                    Codigo = item.Codigo,
                    NombreCompleto = item.NombreCompleto
                };
                listaInspectors.Add(tubero);
            }
            return listaInspectors;
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
        private List<Defectos> ObtenerListaDefectos(List<Sam3_Steelgo_Get_Defectos_Result> listaDefecto)
        {
            List<Defectos> listaDefectos = new List<Defectos>();

            listaDefectos.Add(new Defectos
            {
                DefectoID = 0,
                Nombre = ""
            });

            foreach (Sam3_Steelgo_Get_Defectos_Result item in listaDefecto)
            {
                Defectos Defecto = new Defectos
                {
                    DefectoID = item.DefectoID,
                    Nombre = item.Nombre
                };
                listaDefectos.Add(Defecto);
            }
            return listaDefectos;
        }
        private List<Resultado> ObtenerListaResultado(List<Sam3_Steelgo_Get_TipoResultado_Result> listaResultado)
        {
            List<Resultado> listaResultados = new List<Resultado>();

            listaResultados.Add(new Resultado
            {
                _Resultado = "",
                _ResultadoID = "0"
            });

            foreach (Sam3_Steelgo_Get_TipoResultado_Result item in listaResultado)
            {
                Resultado resultado = new Resultado
                {
                    _Resultado = item.Resultado,
                    _ResultadoID = item.TipoResultadoID.ToString()
                };
                listaResultados.Add(resultado);
            }
            return listaResultados;
        }
        public object Post(Captura listaCapturaInspeccion, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleListas = null;
                DataTable dtDetalleCaptura = null;
                DataTable dtNumerosUnicosAsignados = null;

                foreach (DetalleGuardarInspeccionVisual item in listaCapturaInspeccion.Detalles[0].ListaDetalleGuardarInspeccionVisual)
                {
                    if (item.ListaNumeroUnicoAsignado != null)
                    {
                        if (dtNumerosUnicosAsignados == null)
                            dtNumerosUnicosAsignados = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaNumeroUnicoAsignado);
                        else
                            dtNumerosUnicosAsignados.Merge(Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaNumeroUnicoAsignado));
                    }
                }

                if (listaCapturaInspeccion.Detalles[0].ListaDetalleGuardarInspeccionVisual != null)
                    dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaInspeccion.Detalles[0].ListaDetalleGuardarInspeccionVisual);

                if (listaCapturaInspeccion.Detalles[0].ListaJuntas != null)
                    dtDetalleListas = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaInspeccion.Detalles[0].ListaJuntas);

                dtDetalleCaptura.Columns.Remove("ListaNumeroUnicoAsignado");

                return VisualDimensionalBD.Instance.InsertarCapturaInspeccion(dtDetalleCaptura, dtDetalleListas, dtNumerosUnicosAsignados, usuario.UsuarioID, lenguaje, listaCapturaInspeccion.Detalles[0].InspeccionDimensionalID, listaCapturaInspeccion.Detalles[0].OrdenTrabajoSpoolID, listaCapturaInspeccion.Detalles[0].FechaInspeccion, listaCapturaInspeccion.Detalles[0].ResultadoID, listaCapturaInspeccion.Detalles[0].ObreroID, listaCapturaInspeccion.Detalles[0].DefectoID, listaCapturaInspeccion.Detalles[0].Accion);
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

        public object Get(string ordenTrabajo, string id, string sinCaptura, string token, int todos)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<Sam3_Steelgo_Get_JuntaSpool_Result> juntas = (List<Sam3_Steelgo_Get_JuntaSpool_Result>)VisualDimensionalBD.Instance.ObtenerJuntasXSpoolID(usuario, ordenTrabajo, id, sinCaptura == "Todos" ? 1 : 0);

                List<JuntaXSpoolIDModeloJunta> juntaCorrecta = new List<JuntaXSpoolIDModeloJunta>();
                foreach (Sam3_Steelgo_Get_JuntaSpool_Result item in juntas)
                {
                    juntaCorrecta.Add(new JuntaXSpoolIDModeloJunta
                    {
                        Junta = item.Etiqueta,
                        JuntaID = item.JuntaSpoolID
                    });
                }

                return juntaCorrecta;
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

        public object Get(int id, int sinCaptura, string token, string lenguaje)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DetalleJuntaDimension detalleJuntaDimension = new DetalleJuntaDimension();

                List<Sam3_Inspeccion_Get_DetalleDimensional_Result> listaObtenerDetalleDimensional = (List<Sam3_Inspeccion_Get_DetalleDimensional_Result>)CapturasRapidasBd.Instance.ObtenerDetalleDimensional(id, lenguaje);

                List<DetalleDimensional> listaDetalleDimensional = new List<DetalleDimensional>();

                List<Sam3_Inspeccion_VD_Get_JuntasXSpoolID_Result> listaJuntasPorOrdenTrabajo = (List<Sam3_Inspeccion_VD_Get_JuntasXSpoolID_Result>)VisualDimensionalBD.Instance.ObtenerJuntasVisualDimensional(id.ToString(), usuario, lenguaje, sinCaptura);

                List<InspeccionDimensional.JuntaXSpool> listJuntaXSpool = new List<InspeccionDimensional.JuntaXSpool>();

                foreach (Sam3_Inspeccion_VD_Get_JuntasXSpoolID_Result item in listaJuntasPorOrdenTrabajo)
                {
                    listJuntaXSpool.Add(new InspeccionDimensional.JuntaXSpool
                    {
                        Accion = 1,//por estar solo en la lista habilitado para seleccionar.
                        Junta = item.Etiqueta,
                        JuntaID = item.JuntaSpoolID
                    });
                }

                if (listaObtenerDetalleDimensional.Count > 0)
                {
                    foreach (Sam3_Inspeccion_Get_DetalleDimensional_Result item in listaObtenerDetalleDimensional)
                    {
                        DetalleDimensional detalleDimensional = new DetalleDimensional
                        {
                            Defecto = item.Defecto,
                            DefectoID = item.DefectoID.GetValueOrDefault(),
                            FechaInspeccion = item.FechaInspeccion,
                            InspeccionDimensionalID = item.InspeccionDimensionalID,
                            Inspector = item.Inspector,
                            ObreroID = item.ObreroID,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID.GetValueOrDefault(),
                            Resultado = item.Resultado,
                            ResultadoID = item.ResultadoID,
                            ListaJuntas = listJuntaXSpool,
                            ListaJuntasSeleccionadas = ObtenerJuntasSeleccionadas(id.ToString(), item.DefectoID.GetValueOrDefault(), token, lenguaje),
                            ListaJuntasInicial = ObtenerJuntasSeleccionadas(id.ToString(), item.DefectoID.GetValueOrDefault(), token, lenguaje)
                        };
                        detalleDimensional.ListaJuntas = InspeccionDimensionalController.ObtenerJuntasSeleccionadas(detalleDimensional.ListaJuntas, detalleDimensional.ListaJuntasSeleccionadas);

                        listaDetalleDimensional.Add(detalleDimensional);
                    }
                }
                else
                {
                    DetalleDimensional detalleDimensional = new DetalleDimensional
                    {
                        Defecto = "",
                        DefectoID = 0,
                        FechaInspeccion = "",
                        InspeccionDimensionalID = 0,
                        Inspector = "",
                        ObreroID = 0,
                        OrdenTrabajoSpoolID = 0,
                        Resultado = "",
                        ResultadoID = 0,
                        ListaJuntas = listJuntaXSpool,
                        ListaJuntasSeleccionadas = null,
                        ListaJuntasInicial = null
                    };
                    listaDetalleDimensional.Add(detalleDimensional);
                }

                detalleJuntaDimension.ListaDetalleDimensional = listaDetalleDimensional;

                return detalleJuntaDimension;
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

        public List<InspeccionDimensional.JuntaXSpool> ObtenerJuntasSeleccionadas(string ordenTrabajo, int DefectoID, string token, string Lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            List<InspeccionDimensional.JuntaXSpool> juntasSeleccionadas = null;
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                List<int?> ListaJutasSeleccionadasXSpoolID = (List<int?>)VisualDimensionalBD.Instance.ObtenerDetalleJuntaSeleccionada(ordenTrabajo, DefectoID, usuario, Lenguaje);
                juntasSeleccionadas = ObtenerJuntasID(ListaJutasSeleccionadasXSpoolID);
            }
            return juntasSeleccionadas;
        }

        public static List<InspeccionDimensional.JuntaXSpool> ObtenerJuntasID(List<int?> listaJuntasPorOrdenTrabajoSeleccionada)
        {
            List<InspeccionDimensional.JuntaXSpool> listJuntaXSpoolSeleccionado = new List<InspeccionDimensional.JuntaXSpool>();
            foreach (int item in listaJuntasPorOrdenTrabajoSeleccionada)
            {
                listJuntaXSpoolSeleccionado.Add(new InspeccionDimensional.JuntaXSpool
                {
                    Accion = 2,//dos porque existe ya en el defecto.
                    Junta = "",
                    JuntaID = item
                });
            }
            return listJuntaXSpoolSeleccionado;
        }
    }
}
