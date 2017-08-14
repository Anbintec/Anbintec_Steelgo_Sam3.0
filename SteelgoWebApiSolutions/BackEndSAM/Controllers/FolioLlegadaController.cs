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
    public class FolioLlegadaController : ApiController
    {
        // GET api/foliollegada
        public IEnumerable<FolioLlegada> Get()
        {
            List<FolioLlegada> lstFolioLlegada = new List<FolioLlegada>();
            FolioLlegada foliollegada = new FolioLlegada();
            FolioLlegada foliollegada1 = new FolioLlegada();
            FolioLlegada foliollegada2 = new FolioLlegada();

            foliollegada1.FolioLlegadaID = "12";
            foliollegada1.Consecutivo = "21";
            lstFolioLlegada.Add(foliollegada1);

            foliollegada2.FolioLlegadaID = "13";
            foliollegada2.Consecutivo = "22";
            lstFolioLlegada.Add(foliollegada2);

            return lstFolioLlegada;
        }

        // GET api/foliollegada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/foliollegada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/foliollegada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/foliollegada/5
        public void Delete(int id)
        {
        }
    }
}
