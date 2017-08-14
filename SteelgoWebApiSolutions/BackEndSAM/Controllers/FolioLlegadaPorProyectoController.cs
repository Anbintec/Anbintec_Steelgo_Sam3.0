using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEndSAM.Models;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FolioLlegadaPorProyectoController : ApiController
    {
        // GET api/foliollegadaporproyecto
        public IEnumerable<FolioLlegada> Get(string proyectoID, string username, string token)
        {
            List<FolioLlegada> lstFolioLlegada = new List<FolioLlegada>();
            FolioLlegada folioLlegada = new FolioLlegada();
            FolioLlegada folioLlegada1 = new FolioLlegada();
            FolioLlegada folioLlegada2 = new FolioLlegada();

            folioLlegada.FolioLlegadaID = "12";
            folioLlegada.Consecutivo = "12222";
            folioLlegada.ProyectoID=155;
            lstFolioLlegada.Add(folioLlegada);

            folioLlegada1.FolioLlegadaID = "13";
            folioLlegada1.Consecutivo = "12333";
            folioLlegada1.ProyectoID=156;
            lstFolioLlegada.Add(folioLlegada1);

            if (string.IsNullOrEmpty(proyectoID)) {
                return lstFolioLlegada.AsEnumerable();
            }
            else {
                return lstFolioLlegada.Where(x => x.ProyectoID == int.Parse(proyectoID)).AsEnumerable();
            }
        }

        // GET api/foliollegadaporproyecto/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/foliollegadaporproyecto
        public void Post([FromBody]string value)
        {
        }

        // PUT api/foliollegadaporproyecto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/foliollegadaporproyecto/5
        public void Delete(int id)
        {
        }
    }
}
