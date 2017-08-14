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
    public class DummyFamiliaController : ApiController
    {
        // GET api/dummyfamilia
        public IEnumerable<Familia> Get(string username, string token)
        {
            List<Familia> lstfamilia = new List<Familia>();
            Familia familia = new Familia();
            Familia familia1 = new Familia();
            Familia familia2 = new Familia();

            familia.FamiliaAceroID = "67";
            familia.Nombre = "Titanium";
            lstfamilia.Add(familia);

            familia1.FamiliaAceroID = "68";
            familia1.Nombre = "POLYPRO";
            lstfamilia.Add(familia1);

            familia2.FamiliaAceroID = "69";
            familia2.Nombre = "A333-6";
            lstfamilia.Add(familia2);

            return lstfamilia.AsEnumerable();
        }

        // GET api/dummyfamilia/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyfamilia
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyfamilia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyfamilia/5
        public void Delete(int id)
        {
        }
    }
}
