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
    public class DummyIncidenciasController : ApiController
    {
        // GET api/incidencias
        public IEnumerable<Incidencia> Get(string folio, string token)
        {
            List<Incidencia> lstincidencia = new List<Incidencia>();
            Incidencia incidencia = new Incidencia();
            incidencia.FolioIncidenciaID = 12424103;
            incidencia.Descripcion = "Incidencia con tubos";
            lstincidencia.Add(incidencia);

            Incidencia incidencia2 = new Incidencia();
            incidencia2.FolioIncidenciaID = 12424104;
            incidencia2.Descripcion = "Incidencia con accesorios";
            lstincidencia.Add(incidencia2);

            return lstincidencia.AsEnumerable();
        }
    }
}
