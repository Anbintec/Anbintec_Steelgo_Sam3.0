using BackEndSAM.DataAcces.Inspeccion.Dimensional;
using BackEndSAM.DataAcces.Sam3General;
using BackEndSAM.DataAcces.Sam3General.CamposPredeterminados;
using BackEndSAM.DataAcces.Sam3General.Defectos;

using BackEndSAM.Models.Inspeccion.Dimensional;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using BackEndSAM.DataAcces.Sam3General.TipoResultado;
using BackEndSAM.DataAcces.Sam3General.CapturasRapidas;

namespace BackEndSAM.Controllers.Inspeccion.Dimensional
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InspeccionDimensionalController : ApiController
    {



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


                InspeccionDimensional.CamposPred CamposPredeterminados = new InspeccionDimensional.CamposPred();

                CamposPredeterminados = new InspeccionDimensional.CamposPred
                {
                    Fecha = (string)CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado( lenguaje, 24),
                    Resultado = (string)CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado(lenguaje, 25),
                    Llena = (string)CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado( lenguaje, 26),
                    Muestra = (string)CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado(lenguaje, 47)
                };

                return CamposPredeterminados;
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

        public object Get(string JsonCaptura, string token, string Lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                InspeccionDimensional.DetalleDatosJson capturaDatosJson = serializer.Deserialize<InspeccionDimensional.DetalleDatosJson>(JsonCaptura);

                List<InspeccionDimensional.DetalleDatosJson> listaDetalleDatos = new List<InspeccionDimensional.DetalleDatosJson>();

                List<Sam3_Inspeccion_Get_DetalleDimensional_Result> listaObtenerDetalleDimensional = (List<Sam3_Inspeccion_Get_DetalleDimensional_Result>)CapturasRapidasBd.Instance.ObtenerDetalleDimensional(int.Parse(capturaDatosJson.OrdenTrabajoSpoolID), Lenguaje);

                List<Sam3_Steelgo_Get_JuntaSpool_Result> listaJuntasPorOrdenTrabajo = (List<Sam3_Steelgo_Get_JuntaSpool_Result>)InspeccionDimensionalBD.Instance.ObtenerDetalleJunta(int.Parse(capturaDatosJson.OrdenTrabajoSpoolID), usuario, Lenguaje);

                List<InspeccionDimensional.JuntaXSpool> listJuntaXSpool = new List<InspeccionDimensional.JuntaXSpool>();

                foreach (Sam3_Steelgo_Get_JuntaSpool_Result item in listaJuntasPorOrdenTrabajo)
                {
                    listJuntaXSpool.Add(new InspeccionDimensional.JuntaXSpool
                    {
                        Accion = 1,//por estar solo en la lista habilitado para seleccionar.
                        Junta = item.Etiqueta,
                        JuntaID = item.JuntaSpoolID
                    });
                }




                string fecha = (string)CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado(Lenguaje, 16);


                if (listaObtenerDetalleDimensional.Count == 0)
                {
                    InspeccionDimensional.DetalleDatosJson detalleDatos = new InspeccionDimensional.DetalleDatosJson
                    {
                        Accion = 1,
                        InspeccionDimensionalID = 0,
                        ProyectoID = capturaDatosJson.ProyectoID,
                        OrdenTrabajoSpoolID = capturaDatosJson.OrdenTrabajoSpoolID,
                        OrdenTrabajoSpool = capturaDatosJson.OrdenTrabajoSpool,
                        FechaInspeccion = null,
                        DefectosID = "",
                        Defectos = "",
                        DefectoInicialID = "",
                        DefectoInicial = "",
                        ListaDefectos = ObtenerListaDefectos((List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(Lenguaje, "Inspección dimensional")),
                        InspectorID = "",
                        Inspector = "",
                        ListaInspector = ObtenerListaInspector((List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(capturaDatosJson.ProyectoID, 2, "Inspector Visual Dimensional")),
                        ResultadoID = "",
                        Resultado = "",
                        ListaResultados = ObtenerListaResultado((List<Sam3_Steelgo_Get_TipoResultado_Result>)TipoResultadoBd.Instance.ObtenerListadoResultados(Lenguaje)),
                        ListaJuntas = listJuntaXSpool,
                        TemplateRender = Lenguaje == "es-MX" ? "No hay juntas seleccionadas" : "No joins are selected",
                        TIPO = "NoEspecificarJunta",
                        RowOK=true
                    };
                    listaDetalleDatos.Add(detalleDatos);
                }
                else
                {
                    foreach (Sam3_Inspeccion_Get_DetalleDimensional_Result item in listaObtenerDetalleDimensional)
                    {
                        List<int?> ListaJutasSeleccionadasXSpoolID = (List<int?>)InspeccionDimensionalBD.Instance.ObtenerDetalleJuntaSeleccionada(capturaDatosJson.OrdenTrabajoSpoolID, item.DefectoID.GetValueOrDefault(), usuario, Lenguaje);
                        List<InspeccionDimensional.JuntaXSpool> juntasSeleccionadas = ObtenerJuntasID(ListaJutasSeleccionadasXSpoolID);
                        string idiomaMensaje = Lenguaje == "es-MX" ? "Existen ?1 juntas seleccionadas|No hay juntas seleccionadas" : "There are ?1 joint selected|No joins are selected";
                        InspeccionDimensional.DetalleDatosJson detalleDatos = new InspeccionDimensional.DetalleDatosJson
                        {


                            Accion = 2,
                            InspeccionDimensionalID = item.InspeccionDimensionalID,
                            ProyectoID = capturaDatosJson.ProyectoID,
                            OrdenTrabajoSpoolID = capturaDatosJson.OrdenTrabajoSpoolID,
                            OrdenTrabajoSpool = capturaDatosJson.OrdenTrabajoSpool,
                            FechaInspeccion = item.FechaInspeccion,
                            DefectosID = item.DefectoID.GetValueOrDefault().ToString(),
                            Defectos = item.Defecto,
                            DefectoInicialID = item.DefectoID.GetValueOrDefault().ToString(),
                            DefectoInicial = item.Defecto,
                            ListaDefectos = ObtenerListaDefectos((List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(Lenguaje, "Inspección dimensional")),
                            InspectorID = item.ObreroID.ToString(),
                            Inspector = item.Inspector,
                            ListaInspector = ObtenerListaInspector((List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(capturaDatosJson.ProyectoID, 2, "Inspector Visual Dimensional")),
                            ResultadoID = item.ResultadoID.ToString(),
                            Resultado = item.Resultado,
                            ListaResultados = ObtenerListaResultado((List<Sam3_Steelgo_Get_TipoResultado_Result>)TipoResultadoBd.Instance.ObtenerListadoResultados(Lenguaje)),
                            ListaJuntas = listJuntaXSpool,
                            IDDEFECTOTIPO = item.IdDefectoTipo,
                            ListaJuntasSeleccionadas = juntasSeleccionadas,
                            ListaJuntasSeleccionadasInicial = juntasSeleccionadas,
                            TemplateRender = juntasSeleccionadas.Count > 0 ? idiomaMensaje.Split('|')[0].Replace("?1", juntasSeleccionadas.Count.ToString()) : idiomaMensaje.Split('|')[1],
                            TIPO = item.Tipo,
                            RowOK=true
                        };


                        detalleDatos.ListaJuntas = ObtenerJuntasSeleccionadas(detalleDatos.ListaJuntas, detalleDatos.ListaJuntasSeleccionadas);
                        listaDetalleDatos.Add(detalleDatos);
                    }
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

        public static List<InspeccionDimensional.JuntaXSpool> ObtenerJuntasSeleccionadas(List<InspeccionDimensional.JuntaXSpool> listaJuntas, List<InspeccionDimensional.JuntaXSpool> ListaJuntasSeleccionadas)
        {
            for (int i = 0; i < listaJuntas.Count; i++)
            {
                for (int j = 0; j < ListaJuntasSeleccionadas.Count; j++)
                {
                    if (listaJuntas[i].JuntaID == ListaJuntasSeleccionadas[j].JuntaID)
                    {
                        listaJuntas[i].Accion = 2;
                    }
                }
            }

            return listaJuntas;
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

       
        private List<InspeccionDimensional.Defectos> ObtenerListaDefectos(List<Sam3_Steelgo_Get_Defectos_Result> listaDefecto)
        {
            List<InspeccionDimensional.Defectos> listaDefectos = new List<InspeccionDimensional.Defectos>();
            listaDefectos.Add(new InspeccionDimensional.Defectos());
            foreach (Sam3_Steelgo_Get_Defectos_Result item in listaDefecto)
            {
                InspeccionDimensional.Defectos Defecto = new InspeccionDimensional.Defectos
                {
                    DefectoID = item.DefectoID,
                    Nombre = item.Nombre,
                    IDDEFECTOTIPO = item.IdDefectoTipo,
                    TIPO = item.Tipo
                };
                listaDefectos.Add(Defecto);
            }
            return listaDefectos;
        }
        private List<InspeccionDimensional.Resultado> ObtenerListaResultado(List<Sam3_Steelgo_Get_TipoResultado_Result> listaResultado)
        {
            List<InspeccionDimensional.Resultado> listaResultados = new List<InspeccionDimensional.Resultado>();
            listaResultados.Add(new InspeccionDimensional.Resultado());
            foreach (Sam3_Steelgo_Get_TipoResultado_Result item in listaResultado)
            {
                InspeccionDimensional.Resultado resultado = new InspeccionDimensional.Resultado
                {
                    _Resultado = item.Resultado,
                    _ResultadoID = item.TipoResultadoID.ToString()
                };
                listaResultados.Add(resultado);
            }
            return listaResultados;
        }

        private List<InspeccionDimensional.Inspector> ObtenerListaInspector(List<Sam3_Steelgo_Get_Obrero_Result> listaInspector)
        {
            List<InspeccionDimensional.Inspector> listaInspectors = new List<InspeccionDimensional.Inspector>();
            listaInspectors.Add(new InspeccionDimensional.Inspector());
            foreach (Sam3_Steelgo_Get_Obrero_Result item in listaInspector)
            {
                InspeccionDimensional.Inspector tubero = new InspeccionDimensional.Inspector
                {
                    ObreroID = item.ObreroID,
                    Codigo = item.Codigo
                };
                listaInspectors.Add(tubero);
            }
            return listaInspectors;
        }

        public object Post(InspeccionDimensional.Captura listaCaptura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DataTable JuntasSeleccionadas = null;

                foreach (InspeccionDimensional.DetalleGuardarJson item in listaCaptura.Detalles)
                {
                    if (item.ListaJuntas != null)
                    {
                        if (JuntasSeleccionadas == null)
                            JuntasSeleccionadas = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaJuntas);
                        else
                            JuntasSeleccionadas.Merge(Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaJuntas));

                    }
                }

                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCaptura.Detalles);
                dtDetalleCaptura.Columns.Remove("ListaJuntas");

                return InspeccionDimensionalBD.Instance.InsertarCapturaInspeccion(dtDetalleCaptura, JuntasSeleccionadas, usuario, lenguaje);
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
        public object ObtenerGridGuardado(int OrdenTrabajoSpoolID, string OrdenTrabajoSpool, int ProyectoID, string token, string Lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                List<InspeccionDimensional.JuntaXSpool> listJuntaXSpool = new List<InspeccionDimensional.JuntaXSpool>();
                List<Sam3_Steelgo_Get_JuntaSpool_Result> listaJuntasPorOrdenTrabajo = (List<Sam3_Steelgo_Get_JuntaSpool_Result>)InspeccionDimensionalBD.Instance.ObtenerDetalleJunta(OrdenTrabajoSpoolID, usuario, Lenguaje);
                foreach (Sam3_Steelgo_Get_JuntaSpool_Result item in listaJuntasPorOrdenTrabajo)
                {
                    listJuntaXSpool.Add(new InspeccionDimensional.JuntaXSpool
                    {
                        Accion = 1,//por estar solo en la lista habilitado para seleccionar.
                        Junta = item.Etiqueta,
                        JuntaID = item.JuntaSpoolID
                    });
                }
                List<InspeccionDimensional.DetalleDatosJson> listaDetalleDatos = new List<InspeccionDimensional.DetalleDatosJson>();
                List<Sam3_Inspeccion_Get_DetalleDimensional_Result> listaObtenerDetalleDimensional = (List<Sam3_Inspeccion_Get_DetalleDimensional_Result>)CapturasRapidasBd.Instance.ObtenerDetalleDimensional(OrdenTrabajoSpoolID, Lenguaje);

                if (listaObtenerDetalleDimensional.Count == 0)
                {
                    List<InspeccionDimensional.JuntaXSpool> juntasSeleccionadas = new List<InspeccionDimensional.JuntaXSpool>();
                    InspeccionDimensional.DetalleDatosJson detalleDatos = new InspeccionDimensional.DetalleDatosJson
                    {
                        Accion = 1,
                        InspeccionDimensionalID = 0,
                        ProyectoID = ProyectoID,
                        OrdenTrabajoSpoolID = OrdenTrabajoSpoolID.ToString(),
                        OrdenTrabajoSpool = OrdenTrabajoSpool,
                        FechaInspeccion = null,
                        DefectosID = "",
                        Defectos = "",
                        DefectoInicialID = "",
                        DefectoInicial = "",
                        ListaDefectos = ObtenerListaDefectos((List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(Lenguaje, "Inspección dimensional")),
                        InspectorID = "",
                        Inspector = "",
                        ListaInspector = ObtenerListaInspector((List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(ProyectoID, 2, "Inspector Visual Dimensional")),
                        ResultadoID = "",
                        Resultado = "",
                        ListaResultados = ObtenerListaResultado((List<Sam3_Steelgo_Get_TipoResultado_Result>)TipoResultadoBd.Instance.ObtenerListadoResultados(Lenguaje)),
                        ListaJuntas = listJuntaXSpool,
                        ListaJuntasSeleccionadasInicial = juntasSeleccionadas,
                        ListaJuntasSeleccionadas = juntasSeleccionadas,
                        TemplateRender = Lenguaje == "es-MX" ? "No hay juntas seleccionadas" : "No joins are selected",
                        IDDEFECTOTIPO = 0,
                        TIPO = "NoEspecificarJunta"
                    };
                    listaDetalleDatos.Add(detalleDatos);
                }
                else
                {
                    foreach (Sam3_Inspeccion_Get_DetalleDimensional_Result item in listaObtenerDetalleDimensional)
                    {
                        List<int?> ListaJutasSeleccionadasXSpoolID = (List<int?>)InspeccionDimensionalBD.Instance.ObtenerDetalleJuntaSeleccionada(OrdenTrabajoSpoolID.ToString(), item.DefectoID.GetValueOrDefault(), usuario, Lenguaje);
                        List<InspeccionDimensional.JuntaXSpool> juntasSeleccionadas = ObtenerJuntasID(ListaJutasSeleccionadasXSpoolID);
                        string idiomaMensaje = Lenguaje == "es-MX" ? "Existen ?1 juntas seleccionadas|No hay juntas seleccionadas" : "There are ?1 joint selected|No joints are selected";
                        InspeccionDimensional.DetalleDatosJson detalleDatos = new InspeccionDimensional.DetalleDatosJson
                        {


                            Accion = 2,
                            InspeccionDimensionalID = item.InspeccionDimensionalID,
                            ProyectoID = ProyectoID,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID.ToString(),
                            OrdenTrabajoSpool = OrdenTrabajoSpool,
                            FechaInspeccion = item.FechaInspeccion,
                            DefectosID = item.DefectoID.GetValueOrDefault().ToString(),
                            Defectos = item.Defecto,
                            DefectoInicialID = item.DefectoID.GetValueOrDefault().ToString(),
                            DefectoInicial = item.Defecto,
                            ListaDefectos = ObtenerListaDefectos((List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(Lenguaje, "Inspección dimensional")),
                            InspectorID = item.ObreroID.ToString(),
                            Inspector = item.Inspector,
                            ListaInspector = ObtenerListaInspector((List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(ProyectoID, 2, "Inspector Visual Dimensional")),
                            ResultadoID = item.ResultadoID.ToString(),
                            Resultado = item.Resultado,
                            ListaResultados = ObtenerListaResultado((List<Sam3_Steelgo_Get_TipoResultado_Result>)TipoResultadoBd.Instance.ObtenerListadoResultados(Lenguaje)),
                            ListaJuntas = listJuntaXSpool,
                            IDDEFECTOTIPO = item.IdDefectoTipo,
                            ListaJuntasSeleccionadasInicial = juntasSeleccionadas,
                            ListaJuntasSeleccionadas = juntasSeleccionadas,
                            TemplateRender = juntasSeleccionadas.Count > 0 ? idiomaMensaje.Split('|')[0].Replace("?1", juntasSeleccionadas.Count.ToString()) : idiomaMensaje.Split('|')[1],
                            TIPO = item.Tipo
                        };


                        detalleDatos.ListaJuntas = ObtenerJuntasSeleccionadas(detalleDatos.ListaJuntas, detalleDatos.ListaJuntasSeleccionadas);
                        listaDetalleDatos.Add(detalleDatos);
                    }
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
    }
}
