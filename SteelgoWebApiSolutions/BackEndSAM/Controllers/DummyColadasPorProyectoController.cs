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
    public class DummyColadasPorProyectoController : ApiController
    {
        // GET api/dummycoladasporproyecto
        public IEnumerable<Coladas> Get(int proyectoID, string token)
        {
            List<Coladas> lstColadas = new List<Coladas>();
            Coladas Colada1 = new Coladas();
            Coladas Colada2 = new Coladas();
            Coladas Colada3 = new Coladas();

            if (proyectoID == 1)
            {
                Colada1.ColadaID = 1;
                Colada1.Nombre = "Colada1";
                lstColadas.Add(Colada1);

                Colada2.ColadaID = 2;
                Colada2.Nombre = "Colada2";
                lstColadas.Add(Colada2);

                Colada3.ColadaID = 3;
                Colada3.Nombre = "Colada3";
                lstColadas.Add(Colada3);
            }
            else {
                Colada1.ColadaID = 1;
                Colada1.Nombre = "Colada1";
                lstColadas.Add(Colada1);

                Colada2.ColadaID = 2;
                Colada2.Nombre = "Colada2";
                lstColadas.Add(Colada2);
            }
           

            return lstColadas.AsEnumerable();
        }

        

        // POST api/dummycoladasporproyecto
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummycoladasporproyecto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummycoladasporproyecto/5
        public void Delete(int id)
        {
        }
    }
}
