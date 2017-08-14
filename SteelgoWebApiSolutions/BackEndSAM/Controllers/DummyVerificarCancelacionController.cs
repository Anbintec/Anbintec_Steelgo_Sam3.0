using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BackEndSAM.Models;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyVerificarCancelacionController : ApiController
    {
        // GET api/DummyVerificarCancelacion
        public bool Get(string folio, string username, string token)
        {
            return true;
        }

        // GET api/DummyVerificarCancelacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/DummyVerificarCancelacion
        public void Post([FromBody]string value)
        {
        }

        // PUT api/DummyVerificarCancelacion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/DummyVerificarCancelacion/5
        public void Delete(int id)
        {
        }
    }
}
