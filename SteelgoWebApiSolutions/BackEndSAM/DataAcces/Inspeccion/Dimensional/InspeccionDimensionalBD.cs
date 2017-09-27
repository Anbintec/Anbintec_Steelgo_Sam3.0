using BackEndSAM.Models.Inspeccion.Dimensional;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Inspeccion.Dimensional
{
    public class InspeccionDimensionalBD
    {
        private static readonly object _mutex = new object();
        private static InspeccionDimensionalBD _instance;

        public static InspeccionDimensionalBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new InspeccionDimensionalBD();
                    }
                }
                return _instance;
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

        public object InsertarCapturaInspeccion(DataTable dtDetalleCaptura, DataTable dtDetalleJuntasXSpool, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARINSPECCIONDIMENSIONAL, dtDetalleCaptura, "@Inspeccion", dtDetalleJuntasXSpool, "@juntas", parametro);
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