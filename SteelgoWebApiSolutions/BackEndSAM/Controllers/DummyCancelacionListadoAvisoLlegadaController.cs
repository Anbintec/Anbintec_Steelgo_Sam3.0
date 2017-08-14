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
    public class DummyCancelacionListadoAvisoLlegadaController : ApiController
    {
        // GET api/cancelacionavisollegada
        public IEnumerable<ListadoFolioAvisoLlegada> Get(string folio, string username, string token)
        {
            List<ListadoFolioAvisoLlegada> lstfolioAvisoLlegada = new List<ListadoFolioAvisoLlegada>();
            ListadoFolioAvisoLlegada folioAvisoLlegada = new ListadoFolioAvisoLlegada();
            ListadoFolioAvisoLlegada folioAvisoLlegada1 = new ListadoFolioAvisoLlegada();

            folioAvisoLlegada.FolioAvisoLlegadaID = 1;
            folioAvisoLlegada.NombreProyecto = "Prueba";
            folioAvisoLlegada.FechaRecepcion = DateTime.Today.ToShortDateString();
            folioAvisoLlegada.Patio = "Patio";
            folioAvisoLlegada.Proveedor = "Proveedor";
            folioAvisoLlegada.Transportista = "Transportista";
            folioAvisoLlegada.Estatus = "Prueba";
            lstfolioAvisoLlegada.Add(folioAvisoLlegada);

            DateTime.Today.ToShortTimeString();
            folioAvisoLlegada1.FolioAvisoLlegadaID = 2;
            folioAvisoLlegada1.NombreProyecto = "Prueba 2";
            folioAvisoLlegada1.FechaRecepcion = DateTime.Today.ToShortDateString();
            folioAvisoLlegada1.Patio = "Patio2";
            folioAvisoLlegada1.Proveedor = "Proveedor2";
            folioAvisoLlegada1.Transportista = "Transportista2";
            folioAvisoLlegada1.Estatus = "Cancelacion";
            lstfolioAvisoLlegada.Add(folioAvisoLlegada1);
            
            return lstfolioAvisoLlegada;
        }

        // GET api/cancelacionavisollegada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/cancelacionavisollegada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cancelacionavisollegada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cancelacionavisollegada/5
        public void Delete(int id)
        {
        }
    }
}
