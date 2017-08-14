using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using MessagesManager.Controllers;
using SecurityManager.Api.Models;

namespace BackEndSAM.DataAcces
{
    /// <summary>
    /// Clase con funciones para envio denotificaciones
    /// </summary>
    public class EnviarAvisosBd
    {
        private static readonly object _mutex = new object();
        private static EnviarAvisosBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private EnviarAvisosBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static EnviarAvisosBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EnviarAvisosBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Envia una nueva notificacion
        /// </summary>
        /// <param name="tipoNotificacion"></param>
        /// <param name="mensaje"></param>
        /// <param name="usuario">Información del usuario que lanza este evento</param>
        /// <returns>true / false</returns>
        public object EnviarNotificación(int tipoNotificacion, string mensaje, Sam3_Usuario usuario)
        {
            try
            {
                MessageManagerController mensajero = new MessageManagerController();
                return mensajero.NotificacionesAUsuarios(tipoNotificacion, mensaje, usuario);
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