using BackEndSAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyListadoAvisoLlegadaController : ApiController
    {
        // GET api/dummylistadoavisollegada
        public List<ListadoFolioAvisoLlegada> Get(string prueba,string folio,string username,string token)
        {
            List<ListadoFolioAvisoLlegada> lstfolioAvisoLlegada = new List<ListadoFolioAvisoLlegada>();
            ListadoFolioAvisoLlegada folioAvisoLlegada = new ListadoFolioAvisoLlegada();
            ListadoFolioAvisoLlegada folioAvisoLlegada1 = new ListadoFolioAvisoLlegada();

            if (prueba == "1")
            {
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
                folioAvisoLlegada1.Estatus = "Prueba2";
                lstfolioAvisoLlegada.Add(folioAvisoLlegada1);
            }
            else {
                folioAvisoLlegada.FolioAvisoLlegadaID = 1;
                folioAvisoLlegada.NombreProyecto = "Prueba 1";
                folioAvisoLlegada.FechaRecepcion = DateTime.Today.ToShortDateString();
                lstfolioAvisoLlegada.Add(folioAvisoLlegada);
            }

            return lstfolioAvisoLlegada;
        }

        // POST api/dummylistadoavisollegada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummylistadoavisollegada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummylistadoavisollegada/5
        public string Delete(string folio, string username, string token)
        {
            return "--->" + folio + "<---";
        }
    }
}
