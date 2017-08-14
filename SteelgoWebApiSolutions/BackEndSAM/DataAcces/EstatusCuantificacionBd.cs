using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class EstatusCuantificacionBd
    {
         private static readonly object _mutex = new object();
         private static EstatusCuantificacionBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
         private EstatusCuantificacionBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
         public static EstatusCuantificacionBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EstatusCuantificacionBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolioCuantificacion"></param>
        /// <param name="AvisoEntrada"></param>
        /// <param name="bultoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
         public object CambiarEstatusCuantificacion(int FolioCuantificacion, int AvisoEntrada, int bultoID, Sam3_Usuario usuario)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     if (bultoID != -1)
                     {
                         Sam3_Bulto bulto = ctx.Sam3_Bulto.Where(x=>x.FolioCuantificacionID == FolioCuantificacion && x.BultoID == bultoID && x.Activo).AsParallel().SingleOrDefault();
                         bulto.Estatus = "En Proceso de Recepción";
                         bulto.UsuarioModificacion = usuario.UsuarioID;
                         bulto.FechaModificacion = DateTime.Now;

                         ctx.SaveChanges();
                     }
                     else
                     {
                         Sam3_FolioCuantificacion cuantificacion = (from fc in ctx.Sam3_FolioCuantificacion
                                                                    join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                                    where fc.Activo && fe.Activo
                                                                    && fe.FolioAvisoLlegadaID == AvisoEntrada
                                                                    && fc.FolioCuantificacionID == FolioCuantificacion
                                                                    select fc).AsParallel().SingleOrDefault();

                         cuantificacion.Estatus = "En Proceso de Recepción";
                         cuantificacion.UsuarioModificacion = usuario.UsuarioID;
                         cuantificacion.FechaModificacion = DateTime.Now;

                         ctx.SaveChanges();
                     }

                     TransactionalInformation result = new TransactionalInformation();
                     result.ReturnMessage.Add("Ok");
                     result.ReturnCode = 200;
                     result.ReturnStatus = false;
                     result.IsAuthenicated = true;

                     return result;
                 }
             }
             catch (Exception ex)
             {
                 //-----------------Agregar mensaje al Log -----------------------------------------------
                 LoggerBd.Instance.EscribirLog(ex);
                 //-----------------Agregar mensaje al Log -----------------------------------------------
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