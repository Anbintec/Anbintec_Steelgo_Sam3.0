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
    public class DummyOrdenRecepcionItemCodeController : ApiController
    {
        // GET api/dummyordenrecepcionitemcode
        public IEnumerable<ListaCombos> Get(int folioAvisoEntradID, string token)
        {

            List<ListaCombos> lstFolios = new List<ListaCombos>();
            ListaCombos folios1 = new ListaCombos();
            ListaCombos folios2 = new ListaCombos();
            ListaCombos folios3 = new ListaCombos();

            if (folioAvisoEntradID == 1050)
            {
                folios1.id = "123";
                folios1.value = "IT123";
                lstFolios.Add(folios1);

                folios2.id = "124";
                folios2.value = "IT124";
                lstFolios.Add(folios2);

                folios3.id = "125";
                folios3.value = "IT125";
                lstFolios.Add(folios3);
            }
            else {
                folios1.id = "123";
                folios1.value = "IT123";
                lstFolios.Add(folios1);

                folios2.id = "124";
                folios2.value = "IT124";
                lstFolios.Add(folios2);
            }
            return lstFolios.AsEnumerable();
        }

        // GET api/dummyordenrecepcionitemcode/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyordenrecepcionitemcode
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyordenrecepcionitemcode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyordenrecepcionitemcode/5
        public void Delete(int id)
        {
        }
    }
}
