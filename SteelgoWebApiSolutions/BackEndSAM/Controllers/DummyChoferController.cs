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
    public class DummyChoferController : ApiController
    {
        // GET api/chofer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/chofer/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/chofer
        public void Post(string chofer, string username, string token)
        {
        }

        // PUT api/chofer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/chofer/5
        public void Delete(int id)
        {
        }
    }
}
