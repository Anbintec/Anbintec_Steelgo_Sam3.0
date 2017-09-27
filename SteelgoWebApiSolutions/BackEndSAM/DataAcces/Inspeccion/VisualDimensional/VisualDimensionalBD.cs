using BackEndSAM.Models.Inspeccion.VisualDimensional;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Inspeccion.VisualDimensionalBD
{
    public class VisualDimensionalBD
    {
        private static readonly object _mutex = new object();
        private static VisualDimensionalBD _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private VisualDimensionalBD()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static VisualDimensionalBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new VisualDimensionalBD();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el detalle de la junta con respecto  la inspeccion visual dimensional por cada spool
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <param name="Lenguaje"></param>
        /// <returns></returns>
        public object ObtenerDetalleJunta(CapturaVisualDimensional JsonCaptura, Sam3_Usuario usuario, string Lenguaje, string juntasSeleccionadas)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_InspeccionVisual_Get_Detalle_Result> listaDetalleDatosJson = ctx.Sam3_InspeccionVisual_Get_Detalle(int.Parse(JsonCaptura.OrdenTrabajoSpoolID), Lenguaje).ToList();
                    return listaDetalleDatosJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Obtiene el detalle de la junta con respecto  la inspeccion
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <param name="Lenguaje"></param>
        /// <returns></returns>
        public object ObtenerDetalleJunta(int OrdenTrabajoSpoolID, Sam3_Usuario usuario, string Lenguaje)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Steelgo_Get_JuntaSpool_Result> listaDetalleDatosJson = ctx.Sam3_Steelgo_Get_JuntaSpool(1, OrdenTrabajoSpoolID, null).ToList();
                    return listaDetalleDatosJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        

        /// <summary>
        /// Obtiene las juntas con respecto  la inspeccion visual dimensional
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <param name="Lenguaje"></param>
        /// <returns></returns>
        public object ObtenerJuntasVisualDimensional(string OrdenTrabajoSpoolID, Sam3_Usuario usuario, string Lenguaje, int? todos)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Inspeccion_VD_Get_JuntasXSpoolID_Result> listaDetalleDatosJson = ctx.Sam3_Inspeccion_VD_Get_JuntasXSpoolID(int.Parse(OrdenTrabajoSpoolID), Lenguaje, todos).ToList();
                    return listaDetalleDatosJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Obtiene el detalle de las juntas seleccionadas con respecto a  la inspeccion
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <param name="Lenguaje"></param>
        /// <returns></returns>
        public object ObtenerDetalleJuntaSeleccionada(string OrdenTrabajoSpoolID, int defectoID, Sam3_Usuario usuario, string Lenguaje)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<int?> listaDetalleDatosSeleciconadoJson = ctx.Sam3_Inspeccion_Get_DetalleJuntaSeleccionada(int.Parse(OrdenTrabajoSpoolID), Lenguaje, defectoID).ToList();
                    return listaDetalleDatosSeleciconadoJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        /// <summary>
        /// Obtiene el detalle de las juntas seleccionadas con respecto a  la inspeccion visual dimensional
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <param name="Lenguaje"></param>
        /// <returns></returns>
        public object ObtenerDetalleJuntaSeleccionadaVisualDimensional(string OrdenTrabajoSpoolID, Sam3_Usuario usuario, string Lenguaje)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Inspeccion_VD_Get_JuntasXSpoolID_Result> listaDetalleDatosSeleciconadoJson = ctx.Sam3_Inspeccion_VD_Get_JuntasXSpoolID(int.Parse(OrdenTrabajoSpoolID), Lenguaje, 3).ToList();
                    return listaDetalleDatosSeleciconadoJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
        /// <summary>
        /// Retorna el listado de numeros unicos ¿
        /// </summary>
        /// <param name="JsonCaptura"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object listaNumeroUnicos(int JuntaSpoolID, Sam3_Usuario usuario, int pagina)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Armado_Get_MaterialesSpool_Result> listaDetallaTrabajoAdicionalJson = ctx.Sam3_Armado_Get_MaterialesSpool(JuntaSpoolID, 1, pagina).ToList();
                    return listaDetallaTrabajoAdicionalJson;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerJuntasXSpoolID(Sam3_Usuario usuario, string ordenTrabajo, string id, int sinCaptura)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_JuntaSpool_Result> lista = ctx.Sam3_Steelgo_Get_JuntaSpool(sinCaptura, int.Parse(id), 1).ToList();
                    return lista.OrderBy(x => int.Parse(x.Etiqueta)).ToList<Sam3_Steelgo_Get_JuntaSpool_Result>();
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object InsertarCapturaInspeccion(DataTable dtDetalleCaptura, DataTable dtJuntaLista,DataTable dtNumeroUnicos, int usuario, string lenguaje, int inspeccionDimensionalID, int ordenTrabajoSpoolID, string fechaInspeccion, int resultadoID, int obreroID, int defectoID, int accion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    // ctx.Sam3_Armado_JuntaArmado()
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {
                        { "@Usuario", usuario.ToString() },
                        { "@Lenguaje", lenguaje },
                        { "@InspeccionDimensionalID", inspeccionDimensionalID.ToString() },
                        { "@OrdenTrabajoSpoolID", ordenTrabajoSpoolID.ToString() },
                        { "@FechaInspeccion", fechaInspeccion == null ? "" : fechaInspeccion },
                        { "@ResultadoID", resultadoID.ToString() },
                        { "@ObreroID", obreroID.ToString() },
                        { "@DefectoID", defectoID.ToString() },
                        { "@AccionDimensional", accion.ToString() }

                    };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAINSPECCIONVISUALDIMENSIONAL, dtDetalleCaptura, "@Inspeccion", dtJuntaLista, "@juntas", dtNumeroUnicos, "@NumeroUnicoAsignado", parametro);


                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");

                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}