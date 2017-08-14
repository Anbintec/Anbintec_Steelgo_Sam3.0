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
    public class DummyTransportistaController : ApiController
    {
        // GET api/dummytransportista
        public IEnumerable<Transportista> Get()
        {
            List<Transportista> lstTransportista = new List<Transportista>();
            Transportista transportista = new Transportista();
            transportista.TransportistaID = "123";
            transportista.Nombre = "Francisco Martinez";
            lstTransportista.Add(transportista);

            Transportista transportista1 = new Transportista();
            transportista1.TransportistaID = "124";
            transportista1.Nombre = "Sara Martinez";
            lstTransportista.Add(transportista1);
            return lstTransportista.AsEnumerable();
        }

        // GET api/dummytransportista/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummytransportista
        public void Post(string transportista, string username, string token)
        {
        }

        // PUT api/dummytransportista/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummytransportista/5
        public void Delete(int id)
        {
        }
    }
}
