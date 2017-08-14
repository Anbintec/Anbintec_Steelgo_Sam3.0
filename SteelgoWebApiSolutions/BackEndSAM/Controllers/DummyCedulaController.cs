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
    public class DummyCedulaController : ApiController
    {
        // GET api/cedula
        public IEnumerable<Cedula> Get(string username, string token)
        {
            List<Cedula> lstCedula = new List<Cedula>();
            Cedula cedula = new Cedula();
            Cedula cedula1 = new Cedula();
            Cedula cedula2 = new Cedula();

            cedula.CedulaID = "32";
            cedula.Codigo = "AA11";
            lstCedula.Add(cedula);

            cedula1.CedulaID = "33";
            cedula1.Codigo = "AA12";
            lstCedula.Add(cedula1);

            cedula2.CedulaID = "34";
            cedula2.Codigo = "AA13";
            lstCedula.Add(cedula2);

            return lstCedula.AsEnumerable();

        }

        // GET api/cedula/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/cedula
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cedula/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cedula/5
        public void Delete(int id)
        {
        }
    }
}
