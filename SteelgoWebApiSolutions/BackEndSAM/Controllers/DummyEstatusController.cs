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
    public class DummyEstatusController : ApiController
    {
        // GET api/estatus
        public IEnumerable<Estatus> Get()
        {
            List<Estatus> lstEstatus = new List<Estatus>();
            Estatus status = new Estatus();
            Estatus status1 = new Estatus();
            status.EstatusID = "1";
            status.Nombre = "En Patio";
            lstEstatus.Add(status);

            status1.EstatusID = "2";
            status1.Nombre = "Cierre de Folio Por Devolución";
            lstEstatus.Add(status1);

            return lstEstatus;
        }

        // GET api/estatus/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/estatus
        public void Post([FromBody]string value)
        {
        }

        // PUT api/estatus/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/estatus/5
        public void Delete(int id)
        {
        }
    }
}
