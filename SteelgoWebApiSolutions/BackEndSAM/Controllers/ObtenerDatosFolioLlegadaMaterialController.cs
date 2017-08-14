using System.Collections.Generic;
using System.Web.Http;
using BackEndSAM.DataAcces;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ObtenerDatosFolioLlegadaMaterialController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       /// <summary>
       /// Obtener los datos de un folio cuantificacion
       /// </summary>
       /// <param name="FolioLlegadaMaterialID"></param>
       /// <param name="FolioCuantificacion"></param>
       /// <param name="token"></param>
       /// <returns></returns>
        public object Get(int folioAvisoLlegadaID, int FolioCuantificacion, string detalleBulto, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return FoliosCuantificacionBd.Instance.obtenerDatosFolioCuantificacion(folioAvisoLlegadaID, FolioCuantificacion, detalleBulto);
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
        public object Get( int folioCuantificacionID,  string token)
        {

             string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                return FoliosCuantificacionBd.Instance.ObtenerProyectoFolioCuantificacion(folioCuantificacionID);
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
        
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}