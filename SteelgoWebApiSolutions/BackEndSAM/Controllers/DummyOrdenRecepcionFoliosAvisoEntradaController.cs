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
    public class DummyOrdenRecepcionFoliosAvisoEntradaController : ApiController
    {
        // GET api/dummyordenrecepcionfoliosavisoentrada
        public IEnumerable<ListaCombos> Get(int proyectoid, string token)
        {
            List<ListaCombos> lstFolios = new List<ListaCombos>();
            ListaCombos folios1 = new ListaCombos();
            ListaCombos folios2 = new ListaCombos();
            ListaCombos folios3 = new ListaCombos();
            if (proyectoid == 1)
            {
                folios1.id = "1050";
                folios1.value = "1";
                lstFolios.Add(folios1);

                folios2.id = "1051";
                folios2.value = "2";
                lstFolios.Add(folios2);

                folios3.id = "1052";
                folios3.value = "3";
                lstFolios.Add(folios3);
            }
            else {
                folios1.id = "1050";
                folios1.value = "1";
                lstFolios.Add(folios1);

                folios2.id = "1051";
                folios2.value = "2";
                lstFolios.Add(folios2);
            }
            return lstFolios.AsEnumerable();
        }

        // GET api/dummyordenrecepcionfoliosavisoentrada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyordenrecepcionfoliosavisoentrada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyordenrecepcionfoliosavisoentrada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyordenrecepcionfoliosavisoentrada/5
        public void Delete(int id)
        {
        }
    }
}
