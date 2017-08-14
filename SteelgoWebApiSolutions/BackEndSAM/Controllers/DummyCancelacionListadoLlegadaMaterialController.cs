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
    public class DummyCancelacionListadoLlegadaMaterialController : ApiController
    {
        // GET api/dummycancelacionlistadollegadamaterial
        public IEnumerable<ListadoLlegadaMaterial> Get(string folioavisollegadaid, string foliollegadaid, string username, string token)
        {
            List<ListadoLlegadaMaterial> lstllegadaMat = new List<ListadoLlegadaMaterial>();
            ListadoLlegadaMaterial llegadamaterial = new ListadoLlegadaMaterial();
            ListadoLlegadaMaterial llegadamaterial1 = new ListadoLlegadaMaterial();

            llegadamaterial.FolioLlegadaID = 1;
            llegadamaterial.PackingListID = 12;
            llegadamaterial.FolioAvisoLlegadaID = 2;
            llegadamaterial.Proyecto = "Prueba 1";
            llegadamaterial.Patio = "Patio 1";
            llegadamaterial.Estatus = "En Patio";
            llegadamaterial.EstatusFolioEntrada = "Estatus 1";
            llegadamaterial.EstatusPackingList = "Estatus 2";
            lstllegadaMat.Add(llegadamaterial);


            llegadamaterial1.FolioLlegadaID = 2;
            llegadamaterial1.PackingListID = 13;
            llegadamaterial1.FolioAvisoLlegadaID = 3;
            llegadamaterial1.Proyecto = "Prueba 2";
            llegadamaterial1.Patio = "Patio 2";
            llegadamaterial1.Estatus = "Devolución";
            llegadamaterial1.EstatusFolioEntrada = "Cancelacion";
            llegadamaterial1.EstatusPackingList = "Estatus 2";
            lstllegadaMat.Add(llegadamaterial1);
            return lstllegadaMat;
        }

        // GET api/dummycancelacionlistadollegadamaterial/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummycancelacionlistadollegadamaterial
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummycancelacionlistadollegadamaterial/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummycancelacionlistadollegadamaterial/5
        public void Delete(int id)
        {
        }
    }
}
