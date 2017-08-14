using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.EntidadesPersonalizadas;
using DatabaseManager.Sam3;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClienteController : ApiController
    {
        
        public object Get(string token, int patioID)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return ClienteBd.Instance.ObtenerListadoClientes(usuario, patioID);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Get(string token)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return ClienteBd.Instance.ObtenerListadoClientes(usuario);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }

        
        public object Post(Sam3_TipoArchivo tipoArchivo, string token)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return null;
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }

        
        public object Put(Sam3_TipoArchivo tipoArchivo, string token)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return null;
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }

        
        public object Delete(int id, string token)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return null;
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }
    }
}
