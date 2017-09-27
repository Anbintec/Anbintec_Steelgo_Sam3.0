using BackEndSAM.DataAcces.Embarque.AsignacionEnvio;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Embarque.AsignacionEnvio
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AsignacionEnvioController : ApiController
    {
        [HttpGet]
        public object ObtenerListaEmbarques(string token, int ProyectoID)
        {
            try
            {
                string payload = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    return AsignacionEnvioBD.Instance.ObtenerEmbarques(ProyectoID);
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
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }            
        }

        [HttpGet]
        public object ObtenerDatosPorEmbarqueID(string token, int EmbarqueID, bool Relleno)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if(tokenValido && Relleno)
                {
                    return AsignacionEnvioBD.Instance.ObtenerDatosEmbarques(EmbarqueID);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        [HttpGet]
        public object ObtenerProveedoresEnvio(string token, int ProyectoID, int TipoProveedor)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return AsignacionEnvioBD.Instance.ObtenerProveedorEnvio(ProyectoID, TipoProveedor);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        [HttpGet]
        public object ObtenerTractosEnvio(string token, int ProveedorID, string A, string B)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return AsignacionEnvioBD.Instance.ObtenerTractoEnvio(ProveedorID);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        [HttpGet]
        public object ObtenerChoferEnvio(string token, int ProveedorID, string A, string B, string C)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return AsignacionEnvioBD.Instance.ObtenerChoferEnvio(ProveedorID);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
        [HttpGet]
        public object GuardarCaptura(string token, int EmbarqueID, int ProveedorEnvioID, int TractoEnvioID, int ChoferEnvioID, bool Relleno)
        {
            string payLoad = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (tokenValido)
            {
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payLoad);
                return AsignacionEnvioBD.Instance.GuardarCaptura(EmbarqueID, ProveedorEnvioID, TractoEnvioID, ChoferEnvioID, usuario);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
    }
}
