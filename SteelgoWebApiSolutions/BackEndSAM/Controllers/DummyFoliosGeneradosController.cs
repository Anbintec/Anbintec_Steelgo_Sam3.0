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
    public class DummyFoliosGeneradosController : ApiController
    {
        // GET api/dummyfoliosgenerados
        public IEnumerable<FoliosGenerados> Get()
        {
            List<FoliosGenerados> lstFoliosGenerados = new List<FoliosGenerados>();
            FoliosGenerados foliosGenerados = new FoliosGenerados();
            foliosGenerados.FolioAvisoLlegadaID = 1;
            foliosGenerados.FolioLlegadaID = 2;
            foliosGenerados.PackingListID = 3;
            foliosGenerados.Proyecto = "Proyecto 1";
            lstFoliosGenerados.Add(foliosGenerados);

            return lstFoliosGenerados;
        }

        // GET api/dummyfoliosgenerados/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyfoliosgenerados
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyfoliosgenerados/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyfoliosgenerados/5
        public void Delete(int id)
        {
        }
    }
}
