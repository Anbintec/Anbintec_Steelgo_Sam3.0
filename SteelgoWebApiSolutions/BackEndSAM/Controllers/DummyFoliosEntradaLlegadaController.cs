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
    public class DummyFoliosEntradaLlegadaController : ApiController
    {
        // GET api/dummyfoliosentradallegada
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dummyfoliosentradallegada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyfoliosentradallegada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyfoliosentradallegada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyfoliosentradallegada/5
        public void Delete(int id)
        {
        }
    }
}
