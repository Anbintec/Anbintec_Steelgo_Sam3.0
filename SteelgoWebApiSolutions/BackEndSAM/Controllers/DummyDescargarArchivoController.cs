using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyDescargarArchivoController : ApiController
    {
        // GET api/dummydescargararchivo
        public IEnumerable<string> Get(string archivoID, string username, string token)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dummydescargararchivo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummydescargararchivo
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummydescargararchivo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummydescargararchivo/5
        public void Delete(int id)
        {
        }
    }
}
