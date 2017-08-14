using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BackEndSAM.Models;
using SecurityManager.TokenHandler;
using BackEndSAM.DataAcces;
using SecurityManager.Api.Models;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FamiliaController : ApiController
    {
        // GET api/familia
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/familia/5
        public object Get(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return FamiliaBd.Instance.obtenerFamilia();
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

        // POST api/familia
        public void Post([FromBody]string value)
        {
        }

        // PUT api/familia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/familia/5
        public void Delete(int id)
        {
        }
    }
}
