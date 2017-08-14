using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProxyController : ApiController
    {

        public async Task<object> Get(string url, int proxyID, string token)
        {
            string payload = "";
            string newToken = "";
            HttpContent content = new StringContent("");
            HttpResponseMessage response = new HttpResponseMessage();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                using (HttpClient client = new HttpClient())
                {
                    
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/json");

                    try
                    {
                        using (response = await client.GetAsync(url))
                        {
                            response.EnsureSuccessStatusCode();
                            var resultado = response.Content.ReadAsStringAsync().Result;
                            return resultado;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerBd.Instance.EscribirLog(ex);
                        //-----------------Agregar mensaje al Log -----------------------------------------------
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Ocurrio un error en el proxy.");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;
                        return result;
                    }
                }
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
    }
}
