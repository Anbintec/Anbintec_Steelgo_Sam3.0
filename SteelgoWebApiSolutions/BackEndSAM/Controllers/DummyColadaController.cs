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
    public class DummyColadaController : ApiController
    {
        // GET api/dummycolada
        public IEnumerable<Coladas> Get(string token)
        {
            List<Coladas> lstColadas = new List<Coladas>();
            Coladas coladas1 = new Coladas();
            Coladas coladas2 = new Coladas();
            Coladas coladas3 = new Coladas();

            coladas1.ColadaID = 1;
            coladas1.Nombre = "Coladas 1";
            lstColadas.Add(coladas1);

            coladas2.ColadaID = 2;
            coladas2.Nombre = "Coladas 2";
            lstColadas.Add(coladas2);

            coladas3.ColadaID = 3;
            coladas3.Nombre = "Coladas 3";
            lstColadas.Add(coladas3);

            return lstColadas.AsEnumerable();
        }

        // GET api/dummycolada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummycolada
        public void Post(string data, string token)
        {
        }

        // PUT api/dummycolada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummycolada/5
        public void Delete(int id)
        {
        }
    }
}
