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
    public class TipoArchivoBd
    {
        private static readonly object _mutex = new object();
        private static TipoArchivoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private TipoArchivoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TipoArchivoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoArchivoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoTipoArchivos(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TipoArchivo> lstTipoArchivo = (from r in ctx.Sam3_TipoArchivo
                                                        where r.Activo
                                                        select new TipoArchivo
                                                        {
                                                            Nombre = r.Nombre,
                                                            TipoArchivoID = r.TipoArchivoID.ToString()
                                                        }).AsParallel().ToList();
                    return lstTipoArchivo;
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

        public object obtenerListadoTipoArchivosPorCatalogoID(int catalogoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TipoArchivo> lista = (from ta in ctx.Sam3_TipoArchivo_Catalogo
                                               where ta.Activo && ta.CatalogoID == catalogoID
                                               select new TipoArchivo
                                               {
                                                   Nombre = ta.Nombre,
                                                   TipoArchivoID = ta.TipoArchivoID.ToString()
                                               }).AsParallel().ToList();

                    return lista;
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

        public object InsertarTipoArchivo(Sam3_TipoArchivo nuevo, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_TipoArchivo tipoArchivo = new Sam3_TipoArchivo();
                    tipoArchivo.Activo = true;
                    tipoArchivo.FechaModificacion = DateTime.Now;
                    tipoArchivo.Nombre = nuevo.Nombre;
                    tipoArchivo.UsuarioModificacion = usuario.UsuarioID;

                    ctx.Sam3_TipoArchivo.Add(tipoArchivo);
                    ctx.SaveChanges();

                    return new TipoArchivo { Nombre = tipoArchivo.Nombre, TipoArchivoID = tipoArchivo.TipoArchivoID.ToString() };
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

        public object ActualizarTipoArchivo(Sam3_TipoArchivo cambios, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_TipoArchivo tipoArchivoBd = ctx.Sam3_TipoArchivo.Where(x => x.TipoArchivoID == cambios.TipoArchivoID)
                        .AsParallel().SingleOrDefault();
                    tipoArchivoBd.Nombre = cambios.Nombre;
                    tipoArchivoBd.FechaModificacion = DateTime.Now;
                    tipoArchivoBd.UsuarioModificacion = usuario.UsuarioID;

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

        public object EliminarTipoArchivo(int tipoArchivoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_TipoArchivo tipoArchivoBd = ctx.Sam3_TipoArchivo.Where(x => x.TipoArchivoID == tipoArchivoID)
                        .AsParallel().SingleOrDefault();
                    tipoArchivoBd.Activo = false;
                    tipoArchivoBd.FechaModificacion = DateTime.Now;
                    tipoArchivoBd.UsuarioModificacion = usuario.UsuarioID;

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

    }//Fin clase
}