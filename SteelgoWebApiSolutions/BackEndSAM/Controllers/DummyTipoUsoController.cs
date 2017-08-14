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
    public class DummyTipoUsoController : ApiController
    {
        // GET api/dummytipouso
        public IEnumerable<TipoUso> Get(string token)
        {
            List<TipoUso> lstTipoUso= new List<TipoUso>();

            TipoUso tipouso = new TipoUso();
            tipouso.id = "1";
            tipouso.Nombre = "Tipo Uso 1";
            lstTipoUso.Add(tipouso);

            TipoUso tipouso1 = new TipoUso();
            tipouso1.id = "2";
            tipouso1.Nombre = "Tipo Uso 2";
            lstTipoUso.Add(tipouso1);

            TipoUso tipouso2 = new TipoUso();
            tipouso2.id = "3";
            tipouso2.Nombre = "Tipo Uso 3";
            lstTipoUso.Add(tipouso2);

            return lstTipoUso.AsEnumerable();
        }

        // GET api/dummytipouso/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummytipouso
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummytipouso/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummytipouso/5
        public void Delete(int id)
        {
        }
    }
}
