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
    public class PatioBd
    {
        private static readonly object _mutex = new object();
        private static PatioBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private PatioBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static PatioBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PatioBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerlistadoPatios(string esAvisoEntrada, Sam3_Usuario usuario, int paginaID)
        {
            try
            {
                List<Patio> lstPatios = new List<Patio>();
                using (SamContext ctx = new SamContext())
                {
                    if (int.Parse(esAvisoEntrada) == 1 && (bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Patio", paginaID))
                    {
                        lstPatios.Add(new Patio { Nombre = "Agregar nuevo", PatioID = "0" });
                    }

                    List<int> patios;
                    List<int> proyectos;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<Patio> result = (from p in ctx.Sam3_Patio
                                          where p.Activo && patios.Contains(p.PatioID)
                                          select new Patio
                                          {
                                              Nombre = p.Nombre,
                                              PatioID = p.PatioID.ToString()
                                          }).AsParallel().ToList();

                    lstPatios.AddRange(result);

                    return lstPatios;
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

        public object PatioRequierePermiso(int patioID)
        {
            try
            {
                List<Patio> lstPatios = new List<Patio>();
                using (SamContext ctx = new SamContext())
                {
                    bool result = (from r in ctx.Sam3_Patio
                                   where r.PatioID == patioID
                                   select r.RequierePermisoAduana).AsParallel().SingleOrDefault();

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

        public object InsertarPatio(Sam3_Patio cambios, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Patio.Where(x => x.Nombre == cambios.Nombre && x.Activo).AsParallel().Any())
                    {
                        cambios.Activo = true;
                        cambios.UsuarioModificacion = usuario.UsuarioID;
                        cambios.FechaModificacion = DateTime.Now;
                         
                        ctx.Sam3_Patio.Add(cambios);
                        ctx.SaveChanges();

                        return new Patio { Nombre = cambios.Nombre, PatioID = cambios.PatioID.ToString() };
                    }
                    else
                    {
                        throw new Exception("Patio existente");
                    }
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

        public object ActualizarPatio(Sam3_Patio cambios, Sam3_Usuario Usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if(!ctx.Sam3_Patio.Where(x=> x.Nombre == cambios.Nombre && x.Activo).AsParallel().Any())
                    {
                        Sam3_Patio patioEnBd = ctx.Sam3_Patio.Where(x => x.PatioID == cambios.PatioID && x.Activo).AsParallel().SingleOrDefault();
                        patioEnBd.Activo = cambios.Activo != null && cambios.Activo != patioEnBd.Activo ?
                            cambios.Activo : patioEnBd.Activo;
                        patioEnBd.Descripcion = cambios.Descripcion != null && cambios.Descripcion != patioEnBd.Descripcion ?
                            cambios.Descripcion : patioEnBd.Descripcion;
                        patioEnBd.Nombre = cambios.Nombre != null && cambios.Nombre != patioEnBd.Nombre ?
                            cambios.Nombre : patioEnBd.Nombre;
                        patioEnBd.Propietario = cambios.Propietario != null && cambios.Propietario != patioEnBd.Propietario ?
                            cambios.Propietario : patioEnBd.Propietario;
                        patioEnBd.RequierePermisoAduana = cambios.RequierePermisoAduana != null && cambios.RequierePermisoAduana != patioEnBd.RequierePermisoAduana ?
                            cambios.RequierePermisoAduana : patioEnBd.RequierePermisoAduana;
                        patioEnBd.UsuarioModificacion = Usuario.UsuarioID;
                        patioEnBd.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();

                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("OK");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;

                        return result;
                    }
                    else
                    {
                        throw new Exception("Patio existente");
                    }
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

        public object EliminarPatio(int patioID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Patio patio = ctx.Sam3_Patio.Where(x => x.PatioID == patioID).AsParallel().SingleOrDefault();
                    patio.Activo = false;
                    patio.UsuarioModificacion = usuario.UsuarioID;
                    patio.FechaModificacion = DateTime.Now;

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