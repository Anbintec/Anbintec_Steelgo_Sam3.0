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
    public class DummyPaseSalidaEnviadoController : ApiController
    {
        // GET api/dummypasesalidaenviado
        public bool Get(string folio, string token)
        {
            if (folio == "5")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        // GET api/dummypasesalidaenviado/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummypasesalidaenviado
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummypasesalidaenviado/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummypasesalidaenviado/5
        public void Delete(int id)
        {
        }
    }
}
