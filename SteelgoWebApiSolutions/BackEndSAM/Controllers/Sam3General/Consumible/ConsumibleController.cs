using BackEndSAM.DataAcces.Sam3General.ConsumibleBD;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers.Sam3General.Consumible
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsumibleController : ApiController
    {
        [HttpGet]
        public object ObtieneConsumible(int patioID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return ConsumibleBD.Instance.ObtenerConsumibles(patioID);
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
