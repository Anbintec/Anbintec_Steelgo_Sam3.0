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
    public class DummyPackingListController : ApiController
    {
        // GET api/dummypackinglist
        public IEnumerable<PackingList> Get()
        {
            List<PackingList> lstPackingList = new List<PackingList>();
            PackingList packinglist = new PackingList();
            PackingList packinglist1 = new PackingList();
            packinglist.PackingListID = "1";
            packinglist.Consecutivo = "2";
            lstPackingList.Add(packinglist);

            packinglist1.PackingListID = "2";
            packinglist1.Consecutivo = "3";
            lstPackingList.Add(packinglist1);
            return lstPackingList;
        }

        // GET api/dummypackinglist/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummypackinglist
        public PackingList Post(string foliollegadaid,string proyectoid, string username, string token)
        {
            PackingList packinglist = new PackingList();
            packinglist.PackingListID = "123789456";

            return packinglist;
        }

        // PUT api/dummypackinglist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummypackinglist/5
        public void Delete(int id)
        {
        }
    }
}
