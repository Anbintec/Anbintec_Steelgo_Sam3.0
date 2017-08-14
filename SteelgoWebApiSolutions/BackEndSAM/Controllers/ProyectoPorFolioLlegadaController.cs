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
    public class ProyectoPorFolioLlegadaController : ApiController
    {
        // GET api/proyectoporfoliollegada
        public IEnumerable<Proyecto> Get(string foliollegadaid,string username, string token)
        {

            List<Proyecto> lstProyecto = new List<Proyecto>();
            Proyecto proyecto = new Proyecto();
            Proyecto proyecto1 = new Proyecto();
            Proyecto proyecto2 = new Proyecto();
            Proyecto proyecto3 = new Proyecto();

            if (foliollegadaid == "12") {
                proyecto.ProyectoID = "155";
                proyecto.Nombre = "Proyecto 3";
                lstProyecto.Add(proyecto);
            }

            if (foliollegadaid == "13") {
                proyecto1.ProyectoID = "156";
                proyecto1.Nombre = "Proyecto 4";
                lstProyecto.Add(proyecto1);
            }

            return lstProyecto.AsEnumerable();
        }

        // GET api/proyectoporfoliollegada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/proyectoporfoliollegada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/proyectoporfoliollegada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/proyectoporfoliollegada/5
        public void Delete(int id)
        {
        }
    }
}
