using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using BackEndSAM.Models;
using BackEndSAM.DataAcces;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyAceroController : ApiController
    {
        // GET api/dummyacero
        public IEnumerable<DummyAcero> Get(string token)
        {
            List<DummyAcero> lstAcero = new List<DummyAcero>();
            DummyAcero acero1 = new DummyAcero();
            DummyAcero acero2 = new DummyAcero();
            DummyAcero acero3 = new DummyAcero();

            acero1.AceroID = 1;
            acero1.Nombre = "Acero 1";
            lstAcero.Add(acero1);

            acero2.AceroID = 2;
            acero2.Nombre = "Acero 2";
            lstAcero.Add(acero2);

            acero3.AceroID = 3;
            acero3.Nombre = "Acero 3";
            lstAcero.Add(acero1);

            return lstAcero.AsEnumerable();
        }

        // GET api/dummyacero/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyacero
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyacero/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyacero/5
        public void Delete(int id)
        {
        }
    }
}
