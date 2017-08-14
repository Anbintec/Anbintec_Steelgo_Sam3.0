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
    public class DummyFolioEntradaController : ApiController
    {
        // GET api/dummyfolioentrada
        public IEnumerable<FolioEntrada> Get()
        {
            List<FolioEntrada> lstfolioentrada = new List<FolioEntrada>();
            FolioEntrada folioentrada = new FolioEntrada();
            FolioEntrada folioentrada1 = new FolioEntrada();
            folioentrada.FolioAvisoLlegadaID = "1";
            folioentrada.Consecutivo = "1111";
            lstfolioentrada.Add(folioentrada);
            folioentrada1.FolioAvisoLlegadaID = "2";
            folioentrada1.Consecutivo = "2222";
            lstfolioentrada.Add(folioentrada1);
            return lstfolioentrada.AsEnumerable();
        }

        // GET api/dummyfolioentrada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyfolioentrada
        public FolioLlegada Post(string folioLlegada, string username, string token)
        {
            FolioLlegada _folioLlegada = new FolioLlegada();
            _folioLlegada.FolioLlegadaID = "12";

            return _folioLlegada;
        }

        // PUT api/dummyfolioentrada/5
        public void Put(string folioLlegada, string folioAvisoLlegada, string username, string token)
        {
        }

        // DELETE api/dummyfolioentrada/5
        public void Delete(int id)
        {
        }
    }
}
