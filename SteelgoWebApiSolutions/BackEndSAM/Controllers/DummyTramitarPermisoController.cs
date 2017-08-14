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
    public class DummyTramitarPermisoController : ApiController
    {
        // GET api/dummytramitarpermiso
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dummytramitarpermiso/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummytramitarpermiso
        public void Post(string permiso, string username, string token)
        {
        }

        // PUT api/dummytramitarpermiso/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummytramitarpermiso/5
        public void Delete(int id)
        {
        }
    }
}
