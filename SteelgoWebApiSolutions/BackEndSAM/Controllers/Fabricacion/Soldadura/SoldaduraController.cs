using BackEndSAM.DataAcces.Fabricacion.Soldadura;
using BackEndSAM.DataAcces.Sam3General.ConsumibleBD;
using BackEndSAM.Models.Fabricacion.Soldadura;
using BackEndSAM.Models.Sam3General.Consumible;
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

namespace BackEndSAM.Controllers.Fabricacion.Soldadura
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SoldaduraController : ApiController
    {

        [HttpGet]
        public object getListadoColada(int ProcesoSoldadura, int esRaiz, int WPSID, int FamiliaMaterialID, string lenguaje, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerListadoColadas(ProcesoSoldadura, esRaiz, WPSID, FamiliaMaterialID, lenguaje);
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


        [HttpGet]
        public object RetornaTrabajosAdicionales(int juntaSpoolID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return SoldaduraBD.Instance.ObtenerTrabajosAdicionales(juntaSpoolID);
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

        public object Get(string data)
        {
            //Create a generic return object


            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(/*token*/data, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.AgregarDetalleCapturaSoldadura(usuario);
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

                List<Sam3_Steelgo_Get_SpoolID_Result> lista = (List<Sam3_Steelgo_Get_SpoolID_Result>)SoldaduraBD.Instance.ObtenerIDOrdenTrabajo(usuario, ordenTrabajo, tipo, lenguaje);
                List<IDS> listaAtatus = new List<IDS>();
                listaAtatus.Add( new IDS());
                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        listaAtatus.Add(new IDS { Status = item.status, IDValido = item.ID, Proyecto = item.NombreProyecto, Valor = item.OrdenTrabajoSpoolID, ProyectoID = item.ProyectoID });
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

                string fecha = (string)SoldaduraBD.Instance.ObtenerValorFecha(usuario, lenguaje, 7);

                string muestra = (string)SoldaduraBD.Instance.ObtenerValorFecha(usuario, lenguaje, 14);

                string llena = (string)SoldaduraBD.Instance.ObtenerValorFecha(usuario, lenguaje, 15);

                string tipoCaptura = (string)SoldaduraBD.Instance.ObtenerValorFecha(usuario, lenguaje, 28);

                CamposPredeterminados armadoCamposPredeterminados = new CamposPredeterminados();

                armadoCamposPredeterminados = new CamposPredeterminados
                {
                    FechaSoldadura = fecha,
                    Muestra = muestra,
                    Llena = llena,
                    FormatoFecha = lenguaje == "es-MX" ? "dd/MM/yyyy" : "MM-dd-yyyy",
                    TipoCaptura = tipoCaptura
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
        public object Get(string JsonCaptura, bool isReporte, string lenguaje, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<DetalleDatosJsonSoldadura> listaDetalleDatos = new List<DetalleDatosJsonSoldadura>();
                DetalleCapturaSoldadura capturaDatosJson = serializer.Deserialize<DetalleCapturaSoldadura>(JsonCaptura);
                capturaDatosJson.SinCaptura = capturaDatosJson.SinCaptura == "Todos" ? "1" : "0";

                List<ProcesoSoldadura> ListadoProcesoSoldadura = (List<ProcesoSoldadura>)SoldaduraBD.Instance.ObtenerProcesosSoldadura();
                


                List<Sam3_Steelgo_Get_JuntaSpool_Result> listaJuntasXSpool = null;
                if (isReporte)
                    listaJuntasXSpool = (List<Sam3_Steelgo_Get_JuntaSpool_Result>)SoldaduraBD.Instance.ObtenerJuntasXSpoolID(usuario, capturaDatosJson.IdOrdenTrabajo, capturaDatosJson.idVal, int.Parse(capturaDatosJson.SinCaptura));
                else
                {
                    listaJuntasXSpool = new List<Sam3_Steelgo_Get_JuntaSpool_Result>();
                    listaJuntasXSpool.Add(new Sam3_Steelgo_Get_JuntaSpool_Result());
                }

                for (int i = 0; i < listaJuntasXSpool.Count; i++)
                {
                    if (isReporte)
                    {
                        capturaDatosJson.JuntaID = listaJuntasXSpool[i].JuntaSpoolID.ToString();
                        capturaDatosJson.Junta = listaJuntasXSpool[i].Etiqueta.ToString();
                    }

                    List<Sam3_Soldadura_Get_DetalleJunta_Result> detalle = (List<Sam3_Soldadura_Get_DetalleJunta_Result>)SoldaduraBD.Instance.ObtenerDetalleSoldadura(capturaDatosJson.JuntaID, usuario, lenguaje);

                    IFormatProvider culture = new System.Globalization.CultureInfo("es-MX", true);

                    foreach (Sam3_Soldadura_Get_DetalleJunta_Result item in detalle)
                    {
                        DetalleDatosJsonSoldadura detalleDatos = new DetalleDatosJsonSoldadura
                        {
                            Accion = item.JuntaSoldaduraID == null ? 1 : 2,
                            JuntaSoldaduraID = item.JuntaSoldaduraID.GetValueOrDefault(),
                            IDProyecto = capturaDatosJson.IDProyecto,
                            Proyecto = capturaDatosJson.Proyecto,
                            IdOrdenTrabajo = capturaDatosJson.IdOrdenTrabajo,
                            OrdenTrabajo = capturaDatosJson.OrdenTrabajo,
                            idVal = capturaDatosJson.idVal,
                            idText = capturaDatosJson.idText,
                            Espesor = item.Espesor.GetValueOrDefault(),
                            SpoolID = capturaDatosJson.OrdenTrabajo + "-" + capturaDatosJson.idText,
                            DetalleJunta = "Tipo Junta: " + item.TipoJunta + " - " + "Cedula: " + item.Cedula + " - " + "Localización: " + item.Localizacion,
                            TipoJuntaID = item.TipoJuntaID.ToString(),
                            TipoJunta = item.TipoJunta,
                            JuntaID = capturaDatosJson.JuntaID,
                            Junta = item.Etiqueta,
                            TallerID = item.TallerID.GetValueOrDefault().ToString(),
                            Taller = item.Taller,
                            ListaTaller = ObtenerListaTaller((List<Taller>)SoldaduraBD.Instance.ObtenerTallerXPoryecto(usuario, capturaDatosJson.IDProyecto)),
                            Diametro = Convert.ToDecimal(item.Diametro),
                            FechaSoldadura = item.FechaSoldadura,
                            ListadoColadas = new List<Models.Fabricacion.Soldadura.Consumible>(),   //coladas iguales para raiz y relleno. se le dice consumible por el negocio que se tiene.
                            //Proceso Raiz
                            ProcesoSoldaduraRaizID = item.ProcesoSoldaduraRaizID == null ? 0 : int.Parse(item.ProcesoSoldaduraRaizID.ToString()),
                            ProcesoSoldaduraRaiz = item.CodigoRaiz == null ? "" : item.CodigoRaiz,
                            ListadoProcesoSoldaduraRaiz = ListadoProcesoSoldadura.Where(x => x.Codigo != "N/A").OrderBy(x => x.Codigo).ToList<ProcesoSoldadura>(),
                            //Soldadores Raiz
                            ListaSoldadoresRaizCapturados = (List<Soldadores>)SoldaduraBD.Instance.ObtenerSoldadoresRaizCapturados(capturaDatosJson.IdOrdenTrabajo, capturaDatosJson.idVal, item.JuntaSoldaduraID.GetValueOrDefault(), 1),//el ultimo parametro es el tipo de soldadora o raiz o relleno.
                            ListadoSoldadoresRaiz = new List<ObreroSoldador>(),
                            TemplateSoldadoresRaiz = item.SoldadoresRaiz,
                            //Proceso Relleno
                            ProcesoSoldaduraRellenoID = item.ProcesoSoldaduraRellenoID == null ? 0 : int.Parse(item.ProcesoSoldaduraRellenoID.ToString()),
                            ProcesoSoldaduraRelleno = item.CodigoRelleno == null ? "" : item.CodigoRelleno,
                            ListadoProcesoSoldaduraRelleno = ListadoProcesoSoldadura.OrderBy(x => x.Codigo).ToList<ProcesoSoldadura>(),
                            //Soldadores Relleno
                            ListaSoldadoresRellenoCapturados = (List<Soldadores>)SoldaduraBD.Instance.ObtenerSoldadoresRaizCapturados(capturaDatosJson.IdOrdenTrabajo, capturaDatosJson.idVal, item.JuntaSoldaduraID.GetValueOrDefault(), 0),//el ultimo parametro es el tipo de soldadora o raiz o relleno.
                            ListadoSoldadoresRelleno = new List<ObreroSoldador>(),
                            TemplateSoldadoresRelleno = item.SoldadoresRelleno,
                            //WPS
                            WPSID = item.WPSID.GetValueOrDefault(),
                            WPSNombre = item.WPSNombre,
                            ListaWPS = item.ProcesoSoldaduraRaizID == null || item.ProcesoSoldaduraRellenoID == null ? new List<WPS>() : (List<WPS>)SoldaduraBD.Instance.ObtenerListadoWPS(capturaDatosJson.IDProyecto, item.ProcesoSoldaduraRaizID.GetValueOrDefault(), item.ProcesoSoldaduraRellenoID.GetValueOrDefault(), item.Espesor.GetValueOrDefault(),item.RequierePwht ? 1: 0, lenguaje),
                            //Trabajos adicionales.
                            listaTrabajosAdicionalesSoldadura = (List<TrabajosAdicionalesSoldadura>)SoldaduraBD.Instance.ObtenerTrabajosAdicionales(item.JuntaSpoolID),
                            TemplateTrabajosAdicionales = item.TabajosAdicionales,
                            ListaDetalleTrabajoAdicional = (List<TrabajosAdicionalesSoldadura>)SoldaduraBD.Instance.DetallaSoldaduraAdicional(item.JuntaSoldaduraID.GetValueOrDefault(), usuario),
                            FamiliaMaterialID = item.FamiliaMaterialID,
                            RowOk = true,
                            RequierePwht = item.RequierePwht 
                        };

                        listaDetalleDatos.Add(detalleDatos);
                    }
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

        [HttpGet]
        public object obtenerListadoProcesos(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerProcesosSoldadura();
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

        [HttpGet]
        public object RetornaEdicion(string JsonEditar, string lenguaje, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DetalleDatosJsonSoldadura capturaDatosJson = serializer.Deserialize<DetalleDatosJsonSoldadura>(JsonEditar);
                capturaDatosJson.SinCaptura = capturaDatosJson.SinCaptura == "Todos" ? "1" : "0";
                List<DetalleDatosJsonSoldadura> listaDetalleDatos = new List<DetalleDatosJsonSoldadura>();
                List<Sam3_Soldadura_Get_DetalleJunta_Result> detalle = (List<Sam3_Soldadura_Get_DetalleJunta_Result>)SoldaduraBD.Instance.ObtenerDetalleSoldadura(capturaDatosJson.JuntaID, usuario, lenguaje);
                List<ProcesoSoldadura> ListadoProcesoSoldadura = (List<ProcesoSoldadura>)SoldaduraBD.Instance.ObtenerProcesosSoldadura();

                foreach (Sam3_Soldadura_Get_DetalleJunta_Result item in detalle)
                {
                    DetalleDatosJsonSoldadura detalleDatos = new DetalleDatosJsonSoldadura
                    {
                        Accion = item.JuntaSoldaduraID == null ? 1 : 2,
                        IDProyecto = capturaDatosJson.IDProyecto,
                        Proyecto = capturaDatosJson.Proyecto,
                        IdOrdenTrabajo = capturaDatosJson.IdOrdenTrabajo,
                        OrdenTrabajo = capturaDatosJson.OrdenTrabajo,
                        idVal = capturaDatosJson.idVal,
                        idText = capturaDatosJson.idText,
                        SpoolID = capturaDatosJson.OrdenTrabajo + "-" + capturaDatosJson.idText,
                        DetalleJunta = "Tipo Junta: " + item.TipoJunta + " - " + "Cedula: " + item.Cedula + " - " + "Localización: " + item.Localizacion,
                        TipoJuntaID = item.TipoJuntaID.ToString(),
                        TipoJunta = item.TipoJunta,
                        JuntaID = capturaDatosJson.JuntaID,
                        Junta = capturaDatosJson.Junta,
                        TallerID = item.TallerID.GetValueOrDefault().ToString(),
                        Taller = item.Taller,
                        ListaTaller = ObtenerListaTaller((List<Taller>)SoldaduraBD.Instance.ObtenerTallerXPoryecto(usuario, capturaDatosJson.IDProyecto)),
                        Diametro = Convert.ToDecimal(item.Diametro),
                        FechaSoldadura = item.FechaSoldadura,
                        ListadoColadas = new List<Models.Fabricacion.Soldadura.Consumible>(),   //coladas iguales para raiz y relleno. se le dice consumible por el negocio que se tiene.
                                                                                                //Proceso Raiz
                        ProcesoSoldaduraRaizID = item.ProcesoSoldaduraRaizID == null ? 0 : int.Parse(item.ProcesoSoldaduraRaizID.ToString()),
                        ProcesoSoldaduraRaiz = item.CodigoRaiz == null ? "" : item.CodigoRaiz,
                        ListadoProcesoSoldaduraRaiz = ListadoProcesoSoldadura.Where(x => x.Codigo != "N/A").OrderBy(x => x.Codigo).ToList<ProcesoSoldadura>(),
                        //Soldadores Raiz
                        ListaSoldadoresRaizCapturados = (List<Soldadores>)SoldaduraBD.Instance.ObtenerSoldadoresRaizCapturados(capturaDatosJson.IdOrdenTrabajo, capturaDatosJson.idVal, item.JuntaSoldaduraID.GetValueOrDefault(), 1),//el ultimo parametro es el tipo de soldadora o raiz o relleno.
                        ListadoSoldadoresRaiz = new List<ObreroSoldador>(),
                        TemplateSoldadoresRaiz = item.SoldadoresRaiz,
                        //Proceso Relleno
                        ProcesoSoldaduraRellenoID = item.ProcesoSoldaduraRellenoID == null ? 0 : int.Parse(item.ProcesoSoldaduraRellenoID.ToString()),
                        ProcesoSoldaduraRelleno = item.CodigoRelleno == null ? "" : item.CodigoRelleno,
                        ListadoProcesoSoldaduraRelleno = ListadoProcesoSoldadura.OrderBy(x => x.Codigo).ToList<ProcesoSoldadura>(),
                        //Soldadores Relleno
                        ListaSoldadoresRellenoCapturados = (List<Soldadores>)SoldaduraBD.Instance.ObtenerSoldadoresRaizCapturados(capturaDatosJson.IdOrdenTrabajo, capturaDatosJson.idVal, item.JuntaSoldaduraID.GetValueOrDefault(), 2),//el ultimo parametro es el tipo de soldadora o raiz o relleno.
                        ListadoSoldadoresRelleno = new List<ObreroSoldador>(),
                        TemplateSoldadoresRelleno = item.SoldadoresRelleno,
                        //WPS
                        WPSID = item.WPSID.GetValueOrDefault(),
                        WPSNombre = item.WPSNombre,
                        ListaWPS = item.ProcesoSoldaduraRaizID == null || item.ProcesoSoldaduraRellenoID == null ? new List<WPS>() : (List<WPS>)SoldaduraBD.Instance.ObtenerListadoWPS(capturaDatosJson.IDProyecto, item.ProcesoSoldaduraRaizID.GetValueOrDefault(), item.ProcesoSoldaduraRellenoID.GetValueOrDefault(), item.Espesor.GetValueOrDefault(), item.RequierePwht ? 1 : 0, lenguaje),
                        //Trabajos adicionales.
                        listaTrabajosAdicionalesSoldadura = (List<TrabajosAdicionalesSoldadura>)SoldaduraBD.Instance.ObtenerTrabajosAdicionales(item.JuntaSpoolID),
                        TemplateTrabajosAdicionales = item.TabajosAdicionales,
                        ListaDetalleTrabajoAdicional = (List<TrabajosAdicionalesSoldadura>)SoldaduraBD.Instance.DetallaSoldaduraAdicional(item.JuntaSoldaduraID.GetValueOrDefault(),usuario),  
                        FamiliaMaterialID = item.FamiliaMaterialID,
                        RequierePwht = item.RequierePwht
                    };
                    listaDetalleDatos.Add(detalleDatos);
                }

                return serializer.Serialize(listaDetalleDatos);

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

        //[HttpGet]
        //public object obtenerListadoRenderSoldadores(string token, int procesoSoldaduraID, string tipoJunta, decimal diametro, decimal espesor, string cedula, int proceso, int idProyecto)
        //{
        //    string payload = "";
        //    string newToken = "";
        //    bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //    if (tokenValido)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
        //        return SoldaduraBD.Instance.ObtenerListadoSoldadoresCertificados(procesoSoldaduraID, tipoJunta, diametro, espesor, cedula, proceso, idProyecto);
        //    }
        //    else
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(payload);
        //        result.ReturnCode = 401;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = false;
        //        return result;
        //    }
        //}

        private List<Taller> ObtenerListaTaller(List<Taller> listaTaller)
        {
            List<Taller> listaTalleres = new List<Taller>();
            foreach (Taller item in listaTaller)
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

        public List<TrabajosAdicionalesSoldadura> GenerarTrabajoAdicionalJson(List<Sam3_Soldadura_Get_DetalleTrabajoAdicional_Result> listaTrabajoAdicional, Sam3_Usuario usuario)
        {

            List<TrabajosAdicionalesSoldadura> listaDetalleAdicional = new List<TrabajosAdicionalesSoldadura>();


            foreach (Sam3_Soldadura_Get_DetalleTrabajoAdicional_Result item in listaTrabajoAdicional)
            {
                TrabajosAdicionalesSoldadura detalleAdicional = new TrabajosAdicionalesSoldadura
                {
                    Accion = item.JuntaSoldaduraID == 0 ? 1 : 2,
                    Observacion = item.Observacion,
                    SoldaduraTrabajoAdicionalID = item.SoldaduraTrabajoAdicionalID,
                    JuntaSoldaduraID = item.JuntaSoldaduraID,
                    ObreroID = item.ObreroID,
                    TrabajoAdicional = item.TrabajoAdicional,
                    TrabajoAdicionalID = item.TrabajoAdicionalID,
                    Soldador = item.Soldador
                };
                listaDetalleAdicional.Add(detalleAdicional);
            }
            return listaDetalleAdicional;
        }

      



        public object Get(string ordenTrabajo, string id, string sinCaptura, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerJuntasXSpoolID(usuario, ordenTrabajo, id, sinCaptura == "todos" ? 1 : 0);
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

        public object Get(int idProyecto, int tipo, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerTrabajosXProyecto(usuario, idProyecto);
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

        public object Get(int idProyecto, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerTallerXPoryecto(usuario, idProyecto);
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

        public object Get(int idProyecto, string token, string tipo)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerTrabajosXProyecto(usuario, idProyecto);
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

        [HttpGet]
        public object getListadoWPS(int ProyectoID,int ProcesoRaizID, int ProcesoRellenoID, decimal Espesor, int RequierePwht, string lenguaje, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerListadoWPS(ProyectoID, ProcesoRaizID, ProcesoRellenoID, Espesor, RequierePwht, lenguaje);
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

        [HttpGet]
        public object getListadoSoldadoresCertificados(int TipoProceso, int ProcesoSoldadura, decimal Espesor, decimal Diametro, string lenguaje, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return SoldaduraBD.Instance.ObtenerListadoSoldadoresCertificados(TipoProceso, ProcesoSoldadura, Espesor, Diametro, lenguaje);
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


        

        public object Post(Captura listaCapturasSoldadura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                foreach (DetalleGuardarJson item in listaCapturasSoldadura.Detalles)
                {
                    if (item.ProcesoSoldaduraRaizID == 0)
                        item.ProcesoSoldaduraRaizID = null;

                    if (item.ProcesoSoldaduraRellenoID == 0)
                        item.ProcesoSoldaduraRellenoID = null;


                    if (item.ListaDetalleTrabajoAdicional != null)
                    {
                        foreach (DetalleGuardarTrabajoAdicional detalleTrabajoAdicional in item.ListaDetalleTrabajoAdicional)
                        {
                            detalleTrabajoAdicional.JuntaSpoolID = detalleTrabajoAdicional.JuntaSpoolID == 0 ? int.Parse(item.JuntaSpoolID) : detalleTrabajoAdicional.JuntaSpoolID;
                        }
                    }
                    if (item.ListaSoldaduraRaiz != null)
                    {
                        foreach (GuardarSoldaduraSoldado detalleRaizAdicional in item.ListaSoldaduraRaiz)
                        {
                            detalleRaizAdicional.JuntaSpoolID = detalleRaizAdicional.JuntaSpoolID == 0 ? int.Parse(item.JuntaSpoolID) : detalleRaizAdicional.JuntaSpoolID;
                            
                        }
                    }
                    if (item.ListaSoldaduraRelleno != null)
                    {
                        foreach (GuardarSoldaduraSoldado detalleRellenoAdicional in item.ListaSoldaduraRelleno)
                        {
                            detalleRellenoAdicional.JuntaSpoolID = detalleRellenoAdicional.JuntaSpoolID == 0 ? int.Parse(item.JuntaSpoolID) : detalleRellenoAdicional.JuntaSpoolID;
                            
                        }
                    }
                    item.FechaReporte = "";
                }



                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable TabajosAdicionales = null;
                DataTable RaizAdicionales = null;
                DataTable RellenoAdicionales = null;

                foreach (DetalleGuardarJson item in listaCapturasSoldadura.Detalles)
                {
                    if (item.ListaDetalleTrabajoAdicional != null)
                    {
                        if (TabajosAdicionales == null)
                            TabajosAdicionales = ToDataTable(item.ListaDetalleTrabajoAdicional);
                        else
                            TabajosAdicionales.Merge(ToDataTable(item.ListaDetalleTrabajoAdicional));
                    }
                }

                foreach (DetalleGuardarJson item in listaCapturasSoldadura.Detalles)
                {
                    if (item.ListaSoldaduraRaiz != null)
                    {
                        if (RaizAdicionales == null)
                            RaizAdicionales = ToDataTable(item.ListaSoldaduraRaiz);
                        else
                            RaizAdicionales.Merge(ToDataTable(item.ListaSoldaduraRaiz));
                    }
                }

                foreach (DetalleGuardarJson item in listaCapturasSoldadura.Detalles)
                {
                    if (item.ListaSoldaduraRelleno != null)
                    {
                        if (RellenoAdicionales == null)
                            RellenoAdicionales = ToDataTable(item.ListaSoldaduraRelleno);
                        else
                            RellenoAdicionales.Merge(ToDataTable(item.ListaSoldaduraRelleno));
                    }
                }

                DataTable dtDetalleCaptura = null;
                DataTable dt = null;

                if (listaCapturasSoldadura.Detalles != null)
                {
                    dtDetalleCaptura = new DataTable("Detalles");
                    dtDetalleCaptura = ToDataTable(listaCapturasSoldadura.Detalles);
                    if (RellenoAdicionales != null && RaizAdicionales != null)
                    {
                        dt = new DataTable("DTEnvio");
                        RellenoAdicionales.Merge(RaizAdicionales);
                        dt.Merge(RellenoAdicionales);
                    }
                    else if (RellenoAdicionales == null && RaizAdicionales != null)
                    {
                        dt = new DataTable("DTEnvio");
                        dt.Merge(RaizAdicionales);
                    }
                    else if (RellenoAdicionales != null && RaizAdicionales == null)
                    {
                        dt = new DataTable("DTEnvio");
                        dt.Merge(RellenoAdicionales);
                    }
                    else if (RellenoAdicionales == null && dt != null)
                        dt.Merge(RaizAdicionales);
                    else if (RaizAdicionales == null && dt != null)
                        dt.Merge(RellenoAdicionales);
                }

                dtDetalleCaptura.Columns.Remove("ListaDetalleTrabajoAdicional");
                dtDetalleCaptura.Columns.Remove("ListaSoldaduraRaiz");
                dtDetalleCaptura.Columns.Remove("ListaSoldaduraRelleno");
                return SoldaduraBD.Instance.InsertarCapturaSoldadura(dtDetalleCaptura, TabajosAdicionales, dt, usuario, lenguaje);
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

        public static DataTable ToDataTable<T>(List<T> l_oItems)
        {
            DataTable oReturn = new DataTable(typeof(T).Name);
            object[] a_oValues;
            int i;

            //#### Collect the a_oProperties for the passed T
            PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //#### Traverse each oProperty, .Add'ing each .Name/.BaseType into our oReturn value
            //####     NOTE: The call to .BaseType is required as DataTables/DataSets do not support nullable types, so it's non-nullable counterpart Type is required in the .Column definition
            foreach (PropertyInfo oProperty in a_oProperties)
            {
                oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
            }

            //#### Traverse the l_oItems
            foreach (T oItem in l_oItems)
            {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[a_oProperties.Length];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < a_oProperties.Length; i++)
                {
                    a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                }

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }
        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }
    }
}
