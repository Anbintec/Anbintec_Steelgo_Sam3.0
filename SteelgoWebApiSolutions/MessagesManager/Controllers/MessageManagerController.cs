using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using DatabaseManager.SamLogging;
using MessagesManager.Utils;

namespace MessagesManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MessageManagerController : ApiController
    {
        Base64Security dataSecurity = new Base64Security();

        /// <summary>
        /// Obtiene las notificaciones por ID de usuario
        /// </summary>
        /// <param name="id">ID de usuario</param>
        /// <returns>Notificaciones en formato JSON</returns>
        public object Get(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return MessageLibrary.Instance.GetNotificationsByUserID(usuario.UsuarioID);
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

        /// <summary>
        /// Obtener datos de una notificacion
        /// </summary>
        /// <param name="notificacionID">id de la notificacion</param>
        /// <param name="token">token del usuario</param>
        /// <returns></returns>
        public object Get(int notificacionID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return MessageLibrary.Instance.getNotificationsByNotificationID(notificacionID);
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

        /// <summary>
        /// Crea el mensaje en la cola de mensajes
        /// </summary>
        /// <param name="objectEncrypted">Mensaje encriptado</param>
        /// <param name="typeMessage">Tipo de Mensaje (1: Bitácora, 2: Notificación)</param>
        public object Post(string token, string objectEncrypted, string typeMessage)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                string message = dataSecurity.Decode(objectEncrypted);
                int typeMsg = Convert.ToInt32(typeMessage);

                return MessageLibrary.Instance.SendMessageToQueue(message, typeMsg, usuario);
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

        /// <summary>
        /// Cambia Estatus Lectura de una Notificación a Leida
        /// </summary>
        /// <param name="id">ID de notificación</param>
        public object Put(string token, int id)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return MessageLibrary.Instance.MarkNotificationAsRead(id);
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

        /// <summary>
        /// Marca el mensaje como inactivo
        /// </summary>
        /// <param name="token">Token de la sesion de usuario</param>
        /// <param name="id"> id de notificacion</param>
        /// <returns>estatus 500 OK</returns>
        public object Delete(string token, int id)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return MessageLibrary.Instance.DeleteMessage(id);
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

        /// <summary>
        /// Envia una notificación a la cola de Notificaciones y envia correo a los usuarios que tengan el tipo de Notificación recibido
        /// </summary>
        /// <param name="tipoNotificacion">accion realizada</param>
        /// <param name="mensaje">mensaje de la notificacion</param>
        /// <returns>estatus</returns>
        public bool NotificacionesAUsuarios(int tipoNotificacion, string mensaje, Sam3_Usuario usuario)
        {
            string errorInfo = string.Empty;
            Notificacion datosNotificacion = new Notificacion();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string message = "";
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<UsuarioNotificacion> usuarios = new List<UsuarioNotificacion>();
                    //List<UsuarioNotificacion> usuarios = ctx.Sam3_UsuariosNotificaciones
                    //    .Join(ctx.Sam3_Usuario, un => un.UsuarioID, u => u.UsuarioID, (un, u) => new
                    //    {
                    //        un.TipoNotificacionID,
                    //        un.UsuarioID,
                    //        u.NombreUsuario,
                    //        un.Email,
                    //        un.Plantilla
                    //    })
                    //    .Where(x => x.TipoNotificacionID == tipoNotificacion)
                    //    .Select(s => new UsuarioNotificacion
                    //    {
                    //        TipoNotificacionID = s.TipoNotificacionID,
                    //        UsuarioID = s.UsuarioID,
                    //        NombreUsuario = s.NombreUsuario,
                    //        Email = s.Email,
                    //        Plantilla = s.Plantilla
                    //    }).AsParallel().ToList();

                    usuarios = (from us in ctx.Sam3_Usuario
                               join usn in ctx.Sam3_UsuariosNotificaciones on us.UsuarioID equals usn.UsuarioID
                               where usn.TipoNotificacionID == tipoNotificacion && usn.Activo
                               select new UsuarioNotificacion
                               {
                                   TipoNotificacionID = usn.TipoNotificacionID,
                                   Email = usn.Email,
                                   NombreUsuario = (us.Nombre + " " + us.ApellidoPaterno + " " + us.ApellidoMaterno),
                                   UsuarioID = us.UsuarioID,
                                   Plantilla = usn.Plantilla
                               }).ToList();

                    errorInfo += "\nUsuarios : " + serializer.Serialize(usuarios);

                    foreach (var item in usuarios)
                    {
                        //Enviar a Cola de Notificaciones
                        if (item.UsuarioID != null)
                        {
                            datosNotificacion.UsuarioIDReceptor = item.UsuarioID;
                            datosNotificacion.UsuarioIDEmisor = usuario.UsuarioID;
                            datosNotificacion.TipoNotificacionID = item.TipoNotificacionID;
                            datosNotificacion.Mensaje = mensaje;

                            message = MessageLibrary.Instance.convertirObjToJson(datosNotificacion);
                            MessageLibrary.Instance.SendMessageToQueue(message, 2, usuario);
                        }

                        //Enviar Mail
                        if (!String.IsNullOrEmpty(item.Email))
                        {
                            var path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["urlTemplates"] + item.Plantilla);

                            string contenido = "";
                            StringBuilder body = new StringBuilder();
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("mail.sysgo.com.mx", 25);
                            mail.From = new MailAddress("karen.delacruz@steelgo.com");
                            mail.To.Add(item.Email);
                            //Correo Sam2
                            mail.Sender = new MailAddress("automatic@sysgo.com.mx");
                            //Por definir Subject
                            mail.Subject = "Nueva Notificacion";
                            mail.IsBodyHtml = true;
                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                            //Leer archivo
                            string text = System.IO.File.ReadAllText(path);
                            string[] lines = System.IO.File.ReadAllLines(path);

                            foreach (string line in lines)
                            {
                                string regexPattern = "\\<<.*?\\>>";
                                string tagName = "";
                                string fieldName = line;
                                MatchCollection tagMatches = Regex.Matches(line, regexPattern);
                                foreach (Match match in tagMatches)
                                {
                                    tagName = match.ToString();

                                    if (tagName.Contains("nombreUsuario"))
                                        fieldName = fieldName + line.Replace(tagName, item.NombreUsuario);

                                    if (tagName.Contains("mensaje"))
                                        fieldName = fieldName + line.Replace(tagName, mensaje);

                                    if (tagName.Contains("remitente"))
                                        fieldName = fieldName + line.Replace(tagName, usuario.NombreUsuario);
                                }
                                contenido = contenido + fieldName + "<br />";
                            }

                            //HTML del mensaje
                            mail.Body = contenido;

                            SmtpServer.EnableSsl = false;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("automatic@sysgo.com.mx", "S733lg0H0u*");

                            SmtpServer.Send(mail);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
                return false;
            }
        }

        public bool EnviarAbitacora(int tipoActividad, int entidadID, string mensaje, Sam3_Usuario usuario)
        {
            try
            {
                using (SamLogging lgn = new SamLogging())
                {
                    Bitacora bitacora = new Bitacora();
                    string message = "";

                    bitacora.UsuarioId = usuario.UsuarioID;
                    bitacora.TipoActividadID = tipoActividad;
                    bitacora.Mensaje = mensaje;
                    bitacora.EntidadId = entidadID;

                    message = MessageLibrary.Instance.convertirObjToJson(bitacora);
                    MessageLibrary.Instance.SendMessageToQueue(message, 1, usuario);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
                return false;
            }
        }
    }
}