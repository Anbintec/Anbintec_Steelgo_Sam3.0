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
    public class DummyListadoLlegadaMaterialController : ApiController
    {
        // GET api/dummylistadollegadamaterial
        public IEnumerable<ListadoLlegadaMaterial> Get(string prueba, string foliollegada, string username, string token)
        {

            List<ListadoLlegadaMaterial> lstllegadaMat= new List<ListadoLlegadaMaterial>();
            ListadoLlegadaMaterial llegadamaterial= new ListadoLlegadaMaterial();
            ListadoLlegadaMaterial llegadamaterial1 = new ListadoLlegadaMaterial();

            if (prueba == "1")
            {
                llegadamaterial.FolioLlegadaID = 1;
                llegadamaterial.PackingListID = 12;
                llegadamaterial.FolioAvisoLlegadaID = 2;
                llegadamaterial.Proyecto = "Prueba 1";
                llegadamaterial.Patio = "Patio 1";
                llegadamaterial.Estatus = "En Patio";
                llegadamaterial.EstatusFolioEntrada = "Estatus 1";
                llegadamaterial.EstatusPackingList = "Estatus 2";
                llegadamaterial.FechaCreacion = "27/02/2015";
                llegadamaterial.FechaGeneracion = "28/02/2015";
                lstllegadaMat.Add(llegadamaterial);


                llegadamaterial1.FolioLlegadaID = 2;
                llegadamaterial1.PackingListID = 13;
                llegadamaterial1.FolioAvisoLlegadaID = 3;
                llegadamaterial1.Proyecto = "Prueba 2";
                llegadamaterial1.Patio = "Patio 2";
                llegadamaterial1.Estatus = "Devolución";
                llegadamaterial1.EstatusFolioEntrada = "Estatus 1";
                llegadamaterial1.EstatusPackingList = "Estatus 2";
                llegadamaterial1.FechaCreacion = "27/03/2015";
                llegadamaterial1.FechaGeneracion = "28/03/2015";
                lstllegadaMat.Add(llegadamaterial1);
            }
            else 
            {
                llegadamaterial.FolioLlegadaID = 1;
                llegadamaterial.PackingListID = 12;
                llegadamaterial.FolioAvisoLlegadaID = 2;
                llegadamaterial.Proyecto = "Prueba 1";
                llegadamaterial.Patio = "Patio 1";
                llegadamaterial.Estatus = "En Patio";
                llegadamaterial.EstatusFolioEntrada = "Estatus 1";
                llegadamaterial.EstatusPackingList = "Estatus 2";
                llegadamaterial.FechaCreacion = "27/02/2015";
                llegadamaterial.FechaGeneracion = "28/02/2015";
                lstllegadaMat.Add(llegadamaterial);
            }
            return lstllegadaMat;
        }

        // GET api/dummylistadollegadamaterial/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummylistadollegadamaterial
        public void Post(string folioavisollegadaid, string foliollegadaid, string username, string token)
        {

        }

        // PUT api/dummylistadollegadamaterial/5
        public void Put(string folioavisollegadaid, string foliollegadaid, string username, string token)
        {
        }

        // DELETE api/dummylistadollegadamaterial/5
        public void Delete(int id)
        {
        }
    }
}
