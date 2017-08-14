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
    public class DummyCamionController : ApiController
    {
        // GET api/dummycamion
        public IEnumerable<Camion> Get()
        {
            List<Camion> lstcamion = new List<Camion>();

            lstcamion.Add(new Camion { Placas = "Agregar nuevo", CamionID = "0" });

            Camion camion = new Camion();
            camion.CamionID = "12";
            camion.Placas = "Ruta 128";
            lstcamion.Add(camion);

            Camion camion1 = new Camion();
            camion1.CamionID = "13";
            camion1.Placas = "Ruta 134";
            lstcamion.Add(camion1);

            return lstcamion.AsEnumerable();
        }

        // GET api/dummycamion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummycamion
        public void Post(string plana, string username, string token)
        {
        }

        // PUT api/dummycamion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummycamion/5
        public void Delete(int id)
        {
        }
    }
}
