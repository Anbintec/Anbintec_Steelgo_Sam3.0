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
    /// Operaciones de la entidad Contacto
    /// </summary>
    public class ContactoBd
    {
         private static readonly object _mutex = new object();
        private static ContactoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ContactoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ContactoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ContactoBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera un listado de los contactos activos, para mostrarse en un combo box
        /// </summary>
        /// <returns></returns>
        public object ObtenerListadoContacto()
        {
            try
            {
                List<Contacto> lstContactos = new List<Contacto>();
                //lstContactos.Add(new Contacto { ContactoID = "0", Nombre = "Agregar nuevo" });
                using (SamContext ctx = new SamContext())
                {
                    List<Contacto> result = (from r in ctx.Sam3_Contacto
                                             where r.Activo
                                             select new Contacto
                                             {
                                                 Nombre = r.Nombre,
                                                 ContactoID = r.ContactoID.ToString()
                                             }).AsParallel().ToList();

                    lstContactos.AddRange(result);

                    return lstContactos;
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
        /// Inserta un nuevo contacto
        /// </summary>
        /// <param name="cambios"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object InsertatContacto(Sam3_Contacto cambios, Sam3_Usuario usuario)
        {
             try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Contacto nuevoContacto = new Sam3_Contacto();
                    nuevoContacto.Activo = true;
                    nuevoContacto.ApMaterno = cambios.ApMaterno;
                    nuevoContacto.ApPaterno = cambios.ApPaterno;
                    nuevoContacto.CorreoElectronico = cambios.CorreoElectronico;
                    nuevoContacto.FechaModificacion = DateTime.Now;
                    nuevoContacto.Nombre = cambios.Nombre;
                    nuevoContacto.TelefonoCelular = cambios.TelefonoCelular;
                    nuevoContacto.TelefonoOficina = cambios.TelefonoOficina;
                    nuevoContacto.TelefonoParticular = cambios.TelefonoParticular;
                    nuevoContacto.UsuarioModificacion = usuario.UsuarioID;

                    ctx.Sam3_Contacto.Add(nuevoContacto);
                    ctx.SaveChanges();

                    return new Contacto { ContactoID = nuevoContacto.ContactoID.ToString(), Nombre = nuevoContacto.Nombre };
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
        /// Actualiza la informacion de un contacto
        /// </summary>
        /// <param name="cambios"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ActualizarContacto(Sam3_Contacto cambios, Sam3_Usuario usuario)
        {
             try
            {
                using (SamContext ctx = new SamContext())
                {
                    return null;
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
        /// Elimina logicamente un contacto
        /// </summary>
        /// <param name="contactoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarContacto(int contactoID, Sam3_Usuario usuario)
        {
             try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Contacto contactobd = ctx.Sam3_Contacto.Where(x => x.ContactoID == contactoID).AsParallel().SingleOrDefault();
                    contactobd.Activo = false;
                    contactobd.FechaModificacion = DateTime.Now;
                    contactobd.UsuarioModificacion = usuario.UsuarioID;
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