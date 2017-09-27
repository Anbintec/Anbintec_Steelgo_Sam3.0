using BackEndSAM.DataAcces.Sam3General.CamposPredeterminados;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Sam3General.CamposPredeterminados
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CamposPredeterminadosController : ApiController
    {
        /// <summary>
        /// Retorna el valor del campo predeterminado tomando en consideración el identificador del mismo y el lenguaje
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="lenguaje">lenguaje</param>
        /// <param name="id">identificador del campo predeterminado</param>
        /// <returns></returns>
        public object Get(string token, string lenguaje, int id)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return CamposPredeterminadosBD.Instance.ObtenerValorCampoPredeterminado(lenguaje, id);
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