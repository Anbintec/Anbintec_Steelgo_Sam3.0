using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using SecurityManager.Login;

namespace SecurityManager
{
    /// <summary>
    ///     This Class Manages the Authentication Actions 
    ///         GET Method validates a session
    ///         POST Method creates a session
    ///         DELETE Method removes a session
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticationController : ApiController
    {
        Base64Security dataSecurity = new Base64Security();

        /// <summary>
        ///     Receives the username and token to be validated
        ///     Decrypt the received parameters
        ///     Then validates the session associated to that data
        /// </summary>
        /// <param name="username">
        ///     Username associated to the session encrypted in Base64
        /// </param>
        /// <param name="token">
        ///     Token associated to the session encrypted in Base64
        /// </param>
        /// <returns> TransactionalInformation Object in a JSON Response  </returns>
        /// <example> GET securitymanager/api/authentication/ </example>
        public TransactionalInformation Get(string token)
        {
            //Create a generic return object
            TransactionalInformation transaction = new TransactionalInformation();
            transaction.IsAuthenicated = false;
            string payload = "";
            string newToken = "";
            bool validToken = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //dynamic obj = serializer.DeserializeObject(payload);

            if (validToken)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnCode = 200;
                transaction.ReturnMessage.Add(newToken);
                transaction.IsAuthenicated = true;
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnCode = 400;
                transaction.ReturnMessage.Add(payload);
                transaction.IsAuthenicated = false;
            }

            //string json = serializer.Serialize(transaction);

            return transaction;
        }

        /// <summary>
        ///     Receives the username and password to be validated
        ///     Decrypt the received parameters
        ///     Then validates and creates a session associated to that data
        /// </summary>
        /// <param name="username">
        ///     Username associated to the session encrypted in Base64
        /// </param>
        /// <param name="token">
        ///     Password associated to the session encrypted in Base64
        /// </param>
        /// <returns> TransactionalInformation Object in a JSON Response  </returns>
        /// <example> POST securitymanager/api/authentication/ </example>
        //public TransactionalInformation Post(string username, string password)
        //{
        //    username = dataSecurity.Decode(username);
        //    password = dataSecurity.Decode(password);
        //    //Create a generic return object

        //    TransactionalInformation transaction = new TransactionalInformation();
        //    Sam3_Usuario usuario;
        //    string perfil = "";
        //    //Check in data base
        //    using (SamContext ctx = new SamContext())
        //    {
        //        usuario = (from us in ctx.Sam3_Usuario
        //                   where us.NombreUsuario == username && us.ContrasenaHash == password
        //                   select us).AsParallel().SingleOrDefault();
        //    }

            
        //    transaction.IsAuthenicated = false;

        //    if (usuario != null)
        //    {
        //        int user = string.Compare(usuario.NombreUsuario, username, false);
        //        int pass = string.Compare(usuario.ContrasenaHash, password, false);
        //    }

        //    if (usuario != null && string.Compare(usuario.ContrasenaHash, password, false) == 0  && string.Compare(usuario.NombreUsuario, username, false) == 0)
        //    {
        //        string token = ManageTokens.Instance.CreateJwtToken(usuario);
        //        token = token;
        //        transaction.IsAuthenicated = true;
        //        transaction.ReturnMessage.Add(usuario.Nombre + " " + usuario.ApellidoPaterno);
        //        transaction.ReturnMessage.Add(token);
        //        transaction.ReturnCode = 200;
        //        transaction.ReturnStatus = true;
        //    }
        //    else
        //    {
        //        string message = "Datos de usuario no valido";
        //        transaction.ReturnCode = 400;
        //        transaction.ReturnMessage.Add(message);
        //        transaction.IsAuthenicated = false;
        //        transaction.ReturnStatus = false;
        //    }

        //    return transaction;
        //}

        /// <summary>
        ///     Receives the username and token of the session to be removed
        ///     Decrypt the received parameters
        ///     Then validates the data and removes the session
        /// </summary>
        /// <param name="username">
        ///     Username associated to the session encrypted in Base64
        /// </param>
        /// <param name="token">
        ///     Token associated to the session encrypted in Base64
        /// </param>
        /// <returns> TransactionalInformation Object in a JSON Response  </returns>
        /// <example> GET securitymanager/api/authentication/ </example>
        public TransactionalInformation Delete(string username, string token)
        {
            //Create a generic return object
            TransactionalInformation transaction = new TransactionalInformation();
            transaction.IsAuthenicated = false;
            return transaction;
        }

        public TransactionalInformation Post(string username, string password)
        {
            username = dataSecurity.Decode(username);
            password = dataSecurity.Decode(password);

            TransactionalInformation result = LoginSam2.Instance.Login(username, password);

            return result;
        }
    }
}