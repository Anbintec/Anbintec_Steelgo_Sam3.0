using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using DatabaseManager.EntidadesPersonalizadas;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;
using BackEndSAM.Models;
using SecurityManager.Api.Models;

namespace BackEndSAM.DataAcces
{
    public class TipoAvisoBd
    {
        private static readonly object _mutex = new object();
        private static TipoAvisoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private TipoAvisoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TipoAvisoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoAvisoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoTipoAviso(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TipoAviso> lstTipoAviso = (from r in ctx.Sam3_TipoAviso
                                                    where r.Activo
                                                    select new TipoAviso
                                                    {
                                                        Nombre = r.Nombre,
                                                        TipoAvisoID = r.TipoAvisoID.ToString()
                                                    }).AsParallel().ToList();

                    return lstTipoAviso;
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

        public object insertarTipoAviso(Sam3_TipoAviso aviso, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_TipoAviso nuevoAviso = new Sam3_TipoAviso();
                    nuevoAviso.Activo = true;
                    nuevoAviso.FechaModificacion = DateTime.Now;
                    nuevoAviso.Nombre = aviso.Nombre;
                    nuevoAviso.UsuarioModificacion = usuario.UsuarioID;

                    ctx.Sam3_TipoAviso.Add(nuevoAviso);

                    ctx.SaveChanges();

                    return new TipoAviso { Nombre = nuevoAviso.Nombre, TipoAvisoID = nuevoAviso.TipoAvisoID.ToString() };
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

        public object ActualizarTipoAviso(Sam3_TipoAviso aviso, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    Sam3_TipoAviso avisoBd = ctx.Sam3_TipoAviso.Where(x => x.TipoAvisoID == aviso.TipoAvisoID).AsParallel().SingleOrDefault();
                    avisoBd.Nombre = aviso.Nombre;
                    avisoBd.FechaModificacion = DateTime.Now;
                    avisoBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();


                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
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

        public object EliminarTipoAviso(int tipoAvisoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_TipoAviso avisoBd = ctx.Sam3_TipoAviso.Where(x => x.TipoAvisoID == tipoAvisoID).AsParallel().SingleOrDefault();
                    avisoBd.Activo = false;
                    avisoBd.FechaModificacion = DateTime.Now;
                    avisoBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
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


    }// fin clase
}