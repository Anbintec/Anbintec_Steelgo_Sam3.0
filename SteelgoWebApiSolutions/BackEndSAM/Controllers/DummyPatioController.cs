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
    public class DummyPatioController : ApiController
    {
        // GET api/patio
        public IEnumerable<Patio> Get()
        {
            List<Patio> lstpatio = new List<Patio>();
            Patio patio = new Patio();
            Patio patio1 = new Patio();

            patio.PatioID = "1";
            patio.Nombre = "Plaza Real";
            lstpatio.Add(patio);

            patio1.PatioID = "2";
            patio1.Nombre = "Plaza Vasconcelos";
            lstpatio.Add(patio1);

            return lstpatio;
        }

        // GET api/patio/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/patio
        public void Post(string patio, string username, string token)
        {
        }

        // PUT api/patio/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/patio/5
        public void Delete(int id)
        {
        }
    }
}
