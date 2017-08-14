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
    public class DummyTipoPackingListController : ApiController
    {
        // GET api/dummytipopackinglist
        public IEnumerable<TipoPackingList> Get(string token)
        {
            List<TipoPackingList> lsttipoPackinglist = new List<TipoPackingList>();
            TipoPackingList tipoPackinglist = new TipoPackingList();
            tipoPackinglist.id = "1";
            tipoPackinglist.Nombre = "Tubo";
            lsttipoPackinglist.Add(tipoPackinglist);

            TipoPackingList tipoPackinglist1 = new TipoPackingList();
            tipoPackinglist1.id = "2";
            tipoPackinglist1.Nombre = "Accesorio";
            lsttipoPackinglist.Add(tipoPackinglist1);

            return lsttipoPackinglist.AsEnumerable();
        }

        // GET api/dummytipopackinglist/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummytipopackinglist
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummytipopackinglist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummytipopackinglist/5
        public void Delete(int id)
        {
        }
    }
}
