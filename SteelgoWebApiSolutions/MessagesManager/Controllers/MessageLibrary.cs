
using MessagesManager.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam3;
using System.Messaging;
using SecurityManager.Api.Models;

namespace MessagesManager.Controllers
{
    public class MessageLibrary
    {
        private static readonly object _mutex = new object();
        private static MessageLibrary _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private MessageLibrary()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static MessageLibrary Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MessageLibrary();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Método que envía el mensaje a la cola de mensajes dependiendo de su tipo
        /// 1 es tipo Bitácora
        /// 2 es tipo Notificación
        /// </summary>
        /// <param name="message">Texto del mensaje (json)</param>
        /// <param name="typeMessage">Tipo de Mensaje</param>
        public object SendMessageToQueue(string message, int typeMessage, Sam3_Usuario usuario)
        {
            try
            {
                if (typeMessage == 1)//Bitacora
                {
                    //Dirección del servidor para la cola de tipo Bitácora
                    string path = StringsConfiguration.QuequeBitacora;
                    MessageQueue queueBitacora = new MessageQueue(path);
                    queueBitacora.Formatter = new XmlMessageFormatter(new Type[] { typeof(Bitacora) });
                    Bitacora bitacora = convertirObjToObj<Bitacora>(message);
                    bitacora.Fecha = DateTime.Now;
                    //Enviar mensaje a la cola de mensajes
                    queueBitacora.Send(bitacora);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                }
                else if (typeMessage == 2) //Notificacion
                {
                    //Dirección del servidor para la cola de tipo Notificación
                    string path = StringsConfiguration.QuequeNotifications;
                    MessageQueue queueNotifications = new MessageQueue(path);
                    queueNotifications.Formatter = new XmlMessageFormatter(new Type[] { typeof(Notificacion) });
                    Notificacion notification = convertirObjToObj<Notificacion>(message);
                    notification.FechaEnvio = DateTime.Now;
                    notification.UsuarioModificacion = usuario.UsuarioID;
                    notification.FechaModificacion = DateTime.Now;
                    //Enviar mensaje a la cola de mensajes
                    queueNotifications.Send(notification);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Tipo de Mensaje incorrecto ");
                    result.ReturnCode = 500;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Método que convierte JSON a Objeto tipo T
        /// </summary>
        /// <typeparam name="T">Objeto tipo T (Bitácora/Notificación)</typeparam>
        /// <param name="json">Texto del mensaje </param>
        /// <returns>Objeto tipo T (Bitácora/Notificación)</returns>
        public T convertirObjToObj<T>(string json)
        {
            T p = default(T);
            try
            {
                p = JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonSerializationException jsE)
            {
                string error = jsE.Message;
            }

            return p;
        }

        /// <summary>
        /// Método para convertir las Listas de Notificaciones a JSON
        /// </summary>
        /// <typeparam name="T">Objeto tipo Notificacion/typeparam>
        /// <param name="objeto">Lista tipo Notificación</param>
        /// <returns>Lista de notificaciones en formato JSON</returns>
        public string convertirObjToJson<T>(T objeto)
        {
            string json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(objeto);
            }
            catch (JsonSerializationException jsE)
            {
                string error = jsE.Message;
            }

            return json;
        }

        /// <summary>
        /// Método que obtiene las notificaciones de un usuario
        /// Notificaciones Activas , leidas y no leidas
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de Notificaciones</returns>
        public object GetNotificationsByUserID(int userId)
        {
            try
            {
                List<Notificacion> notifications = new List<Notificacion>();

                using (SamContext ctx = new SamContext())
                {
                    notifications = ctx.Sam3_Notificacion
                        .Join(ctx.Sam3_Usuario, 
                        n => n.UsuarioIDEmisor, u => u.UsuarioID,
                              (n, u) => new
                              {
                                  n.NotificacionID,
                                  n.UsuarioIDReceptor,
                                  n.UsuarioIDEmisor,
                                  n.TipoNotificacionID,
                                  n.Mensaje,
                                  n.FechaEnvio,
                                  n.FechaRecepcion,
                                  n.EstatusLectura,
                                  n.Activo,
                                  n.UsuarioModificacion,
                                  n.FechaModificacion,
                                  u.NombreUsuario
                              })
                        .Where(x => x.UsuarioIDReceptor == userId && x.Activo==true)
                        .Select(x => new Notificacion
                        {
                            NotificacionID = x.NotificacionID,
                            UsuarioIDReceptor = x.UsuarioIDReceptor,
                            UsuarioIDEmisor = x.UsuarioIDEmisor,
                            TipoNotificacionID = x.TipoNotificacionID,
                            Mensaje = x.Mensaje,
                            FechaEnvio = x.FechaEnvio,
                            FechaRecepcion = x.FechaRecepcion,
                            EstatusLectura = x.EstatusLectura,
                            Activo = x.Activo,
                            UsuarioModificacion = x.UsuarioModificacion,
                            FechaModificacion = x.FechaModificacion,
                            NombreUsuarioEmisor = x.NombreUsuario
                        }).AsParallel().ToList();
                    return notifications;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Método para marcar una notificación como Leida
        /// </summary>
        /// <param name="notificationID">ID de la notificación</param>
        public object MarkNotificationAsRead(int notificationID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Notificacion notificacion = new Sam3_Notificacion();
                    notificacion = ctx.Sam3_Notificacion.Where(x => x.NotificacionID == notificationID).First();
                    notificacion.EstatusLectura = true;
                    ctx.SaveChanges();
                }

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnCode = 200;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object DeleteMessage(int notificationID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Notificacion notificacion = new Sam3_Notificacion();
                    notificacion = ctx.Sam3_Notificacion.Where(x => x.NotificacionID == notificationID).First();
                    notificacion.Activo = false;
                    ctx.SaveChanges();
                }

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnCode = 200;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object getNotificationsByNotificationID(int notificationID)
        {
             try
            {
                 Notificacion notifications = new Notificacion();
                using (SamContext ctx = new SamContext())
                {
                    notifications = ctx.Sam3_Notificacion
                       .Join(ctx.Sam3_Usuario,
                       n => n.UsuarioIDEmisor, u => u.UsuarioID,
                             (n, u) => new
                             {
                                 n.NotificacionID,
                                 n.UsuarioIDReceptor,
                                 n.UsuarioIDEmisor,
                                 n.TipoNotificacionID,
                                 n.Mensaje,
                                 n.FechaEnvio,
                                 n.FechaRecepcion,
                                 n.EstatusLectura,
                                 n.Activo,
                                 n.UsuarioModificacion,
                                 n.FechaModificacion,
                                 u.NombreUsuario
                             })
                       .Where(x => x.NotificacionID == notificationID)
                       .Select(x => new Notificacion
                       {
                           NotificacionID = x.NotificacionID,
                           UsuarioIDReceptor = x.UsuarioIDReceptor,
                           UsuarioIDEmisor = x.UsuarioIDEmisor,
                           TipoNotificacionID = x.TipoNotificacionID,
                           Mensaje = x.Mensaje,
                           FechaEnvio = x.FechaEnvio,
                           FechaRecepcion = x.FechaRecepcion,
                           EstatusLectura = x.EstatusLectura,
                           Activo = x.Activo,
                           UsuarioModificacion = x.UsuarioModificacion,
                           FechaModificacion = x.FechaModificacion,
                           NombreUsuarioEmisor = x.NombreUsuario
                       }).AsParallel().FirstOrDefault();
                    return notifications;




                }
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex);
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