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
    public class DummyTipoAceroController : ApiController
    {
        // GET api/dummytipoacero
        public IEnumerable<TipoAcero> Get(string token)
        {
            List<TipoAcero> lsttipoacero = new List<TipoAcero>();
            TipoAcero tipoacero = new TipoAcero();
            TipoAcero tipoacero1 = new TipoAcero();
            TipoAcero tipoacero2 = new TipoAcero();
            TipoAcero tipoacero3 = new TipoAcero();

            tipoacero3.AceroID = "-1";
            tipoacero3.Nomenclatura = "Agregar nuevo";
            lsttipoacero.Add(tipoacero3);

            tipoacero.AceroID = "2";
            tipoacero.Nomenclatura = "AASD1";
            lsttipoacero.Add(tipoacero);

            tipoacero1.AceroID = "3";
            tipoacero1.Nomenclatura = "AASD2";
            lsttipoacero.Add(tipoacero1);

            tipoacero2.AceroID = "4";
            tipoacero2.Nomenclatura = "AASD3";
            lsttipoacero.Add(tipoacero2);

            return lsttipoacero.AsEnumerable();
        }

        // GET api/dummytipoacero/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummytipoacero
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummytipoacero/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummytipoacero/5
        public void Delete(int id)
        {
        }
    }
}
