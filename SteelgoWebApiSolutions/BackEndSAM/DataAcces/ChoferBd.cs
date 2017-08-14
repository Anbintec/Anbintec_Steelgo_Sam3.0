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
    /// <summary>
    /// Altas, bajas, cambios acerca del catalago de choferes.
    /// </summary>
    public class ChoferBd
    {
        private static readonly object _mutex = new object();
        private static ChoferBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ChoferBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ChoferBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ChoferBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene un listado de choferes para cargar en combobox.
        /// </summary>
        /// <param name="esAvisoEntrada"></param>
        /// <returns>JSON {"Nombre" : "xxxx", "ChoferID": "000"}</returns>
        public object ObtenerListadoChoferes(string esAvisoEntrada, int paginaID, string idioma, Sam3_Usuario usuario)
        {
            try
            {
                List<Chofer> lstChoferes = new List<Chofer>();
                using (SamContext ctx = new SamContext())
                {
                    if (int.Parse(esAvisoEntrada) == 1 && (bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Chofer", paginaID))
                    {
                        if (idioma == "en-US")
                        {
                            lstChoferes.Add(new Chofer { Nombre = "Add new", ChoferID = "0" });
                        }
                        else
                        {
                            lstChoferes.Add(new Chofer { Nombre = "Agregar nuevo", ChoferID = "0" });
                        }
                    }

                    List<Chofer> result = (from r in ctx.Sam3_Chofer
                                           where r.Activo
                                           select new Chofer
                                           {
                                               Nombre = r.Nombre,
                                               ChoferID = r.ChoferID.ToString()
                                           }).AsParallel().ToList();

                    result = result.OrderBy(x => x.Nombre).ToList();
                    lstChoferes.AddRange(result);

                    return lstChoferes;
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

        /// <summary>
        /// Genera un nuevo registro de tipo Chofer
        /// </summary>
        /// <param name="chofer">Propiedades del nuevo registro</param>
        /// <param name="usuario">Información del usuario</param>
        /// <returns>JSON {"Nombre": "xxxx", "ChoferID":"10"}</returns>
        public object InsertarChofer(Sam3_Chofer chofer, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Chofer.Where(x => x.Nombre == chofer.Nombre && x.TransportistaID == chofer.TransportistaID && x.Activo).AsParallel().Any())
                    {
                        Sam3_Chofer nuevoChofer = new Sam3_Chofer();
                        nuevoChofer.Activo = true;
                        nuevoChofer.FechaModificacion = DateTime.Now;
                        nuevoChofer.Nombre = chofer.Nombre;
                        nuevoChofer.UsuarioModificacion = usuario.UsuarioID;
                        nuevoChofer.TransportistaID = chofer.TransportistaID;

                        ctx.Sam3_Chofer.Add(nuevoChofer);
                        ctx.SaveChanges();

                        return new Chofer { Nombre = chofer.Nombre, ChoferID = chofer.ChoferID.ToString() };
                    }
                    else
                    {
                        throw new Exception("Chofer existente");
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

        /// <summary>
        /// Actualiza la informacion de un chofer en base de datos
        /// </summary>
        /// <param name="chofer"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ActualizarChofer(Sam3_Chofer chofer, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Chofer.Where(x => x.Nombre == chofer.Nombre && x.Activo).AsParallel().Any())
                    {
                        Sam3_Chofer choferEnBd = ctx.Sam3_Chofer.Where(x => x.ChoferID == chofer.ChoferID && x.Activo).AsParallel().SingleOrDefault();
                        choferEnBd.Activo = chofer.Activo != null && chofer.Activo != choferEnBd.Activo ?
                            chofer.Activo : choferEnBd.Activo;

                        choferEnBd.Nombre = chofer.Nombre != null && chofer.Nombre != choferEnBd.Nombre ?
                            chofer.Nombre : choferEnBd.Nombre;

                        choferEnBd.TransportistaID = chofer.TransportistaID != null && chofer.TransportistaID != choferEnBd.TransportistaID ?
                            chofer.TransportistaID : choferEnBd.TransportistaID;

                        choferEnBd.UsuarioModificacion = usuario.UsuarioID;

                        choferEnBd.FechaModificacion = DateTime.Now;

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
                        throw new Exception("Chofer existente");
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

        /// <summary>
        /// Elimina Logicamente el registro de un Chofer
        /// </summary>
        /// <param name="choferID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarChofer(int choferID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Chofer chofer = ctx.Sam3_Chofer.Where(x => x.ChoferID == choferID).AsParallel().SingleOrDefault();

                    chofer.Activo = false;
                    chofer.FechaModificacion = DateTime.Now;
                    chofer.UsuarioModificacion = usuario.UsuarioID;

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

        /// <summary>
        /// Obtiene una lista de Choferes por transportista
        /// </summary>
        /// <param name="transportistaID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerChoferesProTransportista(int transportistaID, int paginaID, string idioma, Sam3_Usuario usuario)
        {
            try
            {
                List<Chofer> lstChofTrans = new List<Chofer>();
                using (SamContext ctx = new SamContext())
                {
                    if (paginaID > 0 && (bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Chofer", paginaID))
                    {
                        if (idioma == "en-US")
                        {
                            lstChofTrans.Add(new Chofer { Nombre = "Add new", ChoferID = "0" });
                        }
                        else
                        {
                            lstChofTrans.Add(new Chofer { Nombre = "Agregar nuevo", ChoferID = "0" });
                        }
                    }

                    List<Chofer> lstChoferes = (from r in ctx.Sam3_Chofer
                                                where r.Activo && r.TransportistaID == transportistaID
                                                select new Chofer
                                                {
                                                    Nombre = r.Nombre,
                                                    ChoferID = r.ChoferID.ToString()
                                                }).AsParallel().ToList();

                    lstChoferes = lstChoferes.GroupBy(x => x.ChoferID).Select(x => x.First()).ToList();

                    lstChofTrans.AddRange(lstChoferes);
                    return lstChofTrans;

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