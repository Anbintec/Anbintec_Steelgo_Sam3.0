using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEndSAM.Models;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TipoVehiculoController : ApiController
    {
        // GET api/tipovehiculo
        public IEnumerable<TipoVehiculo> Get(string token)
        {
            List<TipoVehiculo> lstTipoVehiculo = new List<TipoVehiculo>();
            TipoVehiculo tipovehiculo = new TipoVehiculo();
            tipovehiculo.TipoVehiculoID = "1";
            tipovehiculo.Nombre = "Tracto";
            lstTipoVehiculo.Add(tipovehiculo);

            TipoVehiculo tipovehiculo1 = new TipoVehiculo();
            tipovehiculo1.TipoVehiculoID = "2";
            tipovehiculo1.Nombre = "Plana";
            lstTipoVehiculo.Add(tipovehiculo1);
            
            return lstTipoVehiculo.AsEnumerable();
        }

        // GET api/tipovehiculo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tipovehiculo
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tipovehiculo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tipovehiculo/5
        public void Delete(int id)
        {
        }
    }
}
