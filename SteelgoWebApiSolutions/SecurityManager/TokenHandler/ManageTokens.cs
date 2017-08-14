using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using JWT;
using System.Runtime.Remoting;
using System.Configuration;
using DatabaseManager.Sam3;
using System.Web.Script.Serialization;
using SecurityManager.Login;
using SecurityManager.Api.Models;

namespace SecurityManager.TokenHandler
{
    public class ManageTokens
    {
        private static readonly object _mutex = new object();
        private static ManageTokens _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ManageTokens()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ManageTokens Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ManageTokens();
                    }
                }
                return _instance;
            }
        }


        public string CreateJwtToken(Sam3_Usuario usuario)
        {
            try
            {
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                double now = Math.Round((DateTime.UtcNow.AddMinutes(30) - unixEpoch).TotalSeconds);

                Dictionary<string, object> payload = new Dictionary<string, object>()
            {
                { "UsuarioID", usuario.UsuarioID},
                { "NombreUsuario", usuario.NombreUsuario },
                { "ContrasenaHash", usuario.ContrasenaHash },
                { "PerfilID", usuario.PerfilID},
                { "BloqueadoPorAdministracion", usuario.BloqueadoPorAdministracion},
                { "Activo", usuario.Activo},
                { "exp", now}
            };

                string token = JWT.JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["scrKey"], JWT.JwtHashAlgorithm.HS256);
                return token;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                Logger.Instance.EscribirLog(ex.Message + "Stack: " + ex.StackTrace + "----" + ex.InnerException);
                return ex.Message + "Stack: " + ex.StackTrace + "----" + ex.InnerException;
            }
        }

        public bool ValidateToken(string token, out string result, out string newToken)
        {
            try
            {
                result = JsonWebToken.Decode(token, ConfigurationManager.AppSettings["scrKey"]);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(result);
                //Logger.Instance.EscribirLog("Resultado de validar el token: " + result);
                newToken = CreateJwtToken(usuario);
                return true;
            }
            catch (JWT.SignatureVerificationException ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                Logger.Instance.EscribirLog(ex.InnerException + "Source: " + ex.Source + "stack :" + ex.StackTrace);

                result = ex.InnerException + "Source: " + ex.Source + "stack :" + ex.StackTrace;
                newToken = "";
                return false;
            }
            catch (Exception ex)
            {
                Logger.Instance.EscribirLog(ex.InnerException + "Source: " + ex.Source + "stack :" + ex.StackTrace);
                result = ex.InnerException + "Source: " + ex.Source + "stack :" + ex.StackTrace;
                newToken = "";
                return false;
            }
        }
    }
}