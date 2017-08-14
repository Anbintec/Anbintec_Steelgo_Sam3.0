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
    public class DummyGuardarFolioLlegadaCuantificacionController : ApiController
    {
        // GET api/guardarfoliollegadacuantificacion
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/guardarfoliollegadacuantificacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/guardarfoliollegadacuantificacion
        public int Post(string data)
        {
            return 12;
        }

        // PUT api/guardarfoliollegadacuantificacion/5
        public void Put(string data)
        {

        }

        // DELETE api/guardarfoliollegadacuantificacion/5
        public void Delete(int id)
        {
        }
    }
}
