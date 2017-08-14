using DatabaseManager.Sam3;
using DatabaseManager.SamLogging;
using System;
using System.IO;
using System.Messaging;
using System.Text;

namespace LoggerDaemon
{
    public class LoggerDaemonLibrary
    {
        /// <summary>
        /// Método para Leer los mensajes de la cola de Mensajes tipo Notificación
        /// e insertarlos en la base de datos
        /// </summary>
        public static void ReadMessagesNotificaciones()
        {
            try
            {
                //Dirección del servidor para los mensajes de tipo Notificacion
                string path = StringsConfiguration.QuequeNotifications;
                MessageQueue mq = new MessageQueue(path);

                foreach (System.Messaging.Message message in mq.GetAllMessages())
                {
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(Notificacion) });
                    Notificacion p = (Notificacion)message.Body;
                    LoggerDaemonLibrary.insertNotification(p);
                }
                //Elimina todos los mensajes de la cola
                mq.Purge();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Método para leer los mensajes de la cola de Mensajes Tipo Bitácora
        /// e insertarlos en la base de datos
        /// </summary>
        public static void ReadMessagesBitacora()
        {
            try
            {
                //Dirección del servidor para los mensajes de tipo Bitácora
                string path = StringsConfiguration.QuequeBitacora;
                MessageQueue mq = new MessageQueue(path);

                foreach (System.Messaging.Message message in mq.GetAllMessages())
                {
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(Bitacora) });
                    Bitacora p = (Bitacora)message.Body;
                    LoggerDaemonLibrary.insertBitacora(p);
                }
                //Elimina todos los mensajes de la cola
                mq.Purge();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Método para Insertar el mensaje de tipo Notificación obtenido de la cola de mensajes
        /// a la Base de datos
        /// </summary>
        /// <param name="notification">Mensaje tipo Notificacion</param>
        public static void insertNotification(Notificacion notification)
        {
            using (SamContext ctx = new SamContext())
            {
                Sam3_Notificacion noti = new Sam3_Notificacion();
                //noti.NotificacionID = notification.NotificacionID;
                noti.UsuarioIDReceptor = notification.UsuarioIDReceptor;
                noti.UsuarioIDEmisor = notification.UsuarioIDEmisor;
                noti.TipoNotificacionID = notification.TipoNotificacionID;
                noti.Mensaje = notification.Mensaje;
                noti.FechaEnvio = notification.FechaEnvio;
                noti.FechaRecepcion = DateTime.Now; //????
                noti.EstatusLectura = false;
                noti.Activo = true;
                noti.UsuarioModificacion = notification.UsuarioModificacion;
                noti.FechaModificacion = notification.FechaModificacion;

                ctx.Sam3_Notificacion.Add(noti);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Método para insertar el mensaje de Tipo Bitácora obtenido de la cola de mensajes
        /// a la base de datos
        /// </summary>
        /// <param name="log">Mensaje Tipo Bitácora</param>
        public static void insertBitacora(Bitacora log)
        {
            using (SamLogging ctx = new SamLogging())
            {
                DatabaseManager.SamLogging.Bitacora bitacora = new DatabaseManager.SamLogging.Bitacora();
                //bitacora.BitacoraId = log.BitacoraId;
                bitacora.UsuarioId = log.UsuarioId;
                bitacora.TipoActividadID = log.TipoActividadID;
                bitacora.Mensaje = log.Mensaje;
                bitacora.Fecha = log.Fecha;
                bitacora.EntidadId = log.EntidadId;

                ctx.Bitacora.Add(bitacora);
                ctx.SaveChanges();
            }
        }
    }
}
