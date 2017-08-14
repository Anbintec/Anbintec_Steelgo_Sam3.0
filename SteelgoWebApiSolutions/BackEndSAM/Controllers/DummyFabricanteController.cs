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
    public class DummyFabricanteController : ApiController
    {
        // GET api/fabricante
        public IEnumerable<DummyFabricante> Get(string token)
        {
            List<DummyFabricante> lstFabricante = new List<DummyFabricante>();
            DummyFabricante fab1 = new DummyFabricante();
            DummyFabricante fab2 = new DummyFabricante();
            DummyFabricante fab3 = new DummyFabricante();

            fab1.FabricanteID = 1;
            fab1.Nombre = "Fabricante 1";
            lstFabricante.Add(fab1);

            fab2.FabricanteID = 2;
            fab2.Nombre = "Fabricante 2";
            lstFabricante.Add(fab2);

            fab3.FabricanteID = 3;
            fab3.Nombre = "Fabricante 3";
            lstFabricante.Add(fab3);

            return lstFabricante.AsEnumerable();
        }

        // POST api/fabricante
        public void Post([FromBody]string value)
        {
        }

        // PUT api/fabricante/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/fabricante/5
        public void Delete(int id)
        {
        }
    }
}
