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
    public class DumyValidarItemCodeNuCuantificacionController : ApiController
    {
        // GET api/dumyvalidaritemcodenucuantificacion
        public bool Get(string folioAvisoLlegadaID, string folioCuantificacionID, string ItemCode, string BultoID,  string token)
        {
            return true;
        }

        // GET api/dumyvalidaritemcodenucuantificacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dumyvalidaritemcodenucuantificacion
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dumyvalidaritemcodenucuantificacion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dumyvalidaritemcodenucuantificacion/5
        public void Delete(string folioAvisoLlegadaID, string folioCuantificacionID, string ItemCode, string token)
        {
        }
    }
}
