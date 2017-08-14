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
    public class DummyPlanaController : ApiController
    {
        // GET api/dummyplana
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dummyplana/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyplana
        public void Post(string plana, string username, string token)
        {
        }

        // PUT api/dummyplana/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyplana/5
        public void Delete(int id)
        {
        }
    }
}
