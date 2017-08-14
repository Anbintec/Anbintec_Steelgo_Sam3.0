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
    public class DummyCambiarEstatusCuantificacionController : ApiController
    {
        // GET api/dummycambiarestatuscuantificacion
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dummycambiarestatuscuantificacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummycambiarestatuscuantificacion
        public void Post(string terminar,string token)
        {
        }

        // PUT api/dummycambiarestatuscuantificacion/5
        public void Put(string folioCuantificacionID, string token)
        {

        }

        // DELETE api/dummycambiarestatuscuantificacion/5
        public void Delete(int id)
        {
        }
    }
}
