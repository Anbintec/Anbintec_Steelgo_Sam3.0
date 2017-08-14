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
    public class DummyEntradaMaterialController : ApiController
    {
        // GET api/entradamaterial
        public EdicionLlegadaMaterial Get(string folioAvisoLlegada,string folioLlegada, string username, string token)
        {
            List<EdicionLlegadaMaterial> lstedit = new List<EdicionLlegadaMaterial>();
            List<Files> lstfiles= new List<Files>();
            List<Files> lstfiles2 = new List<Files>();
            EdicionLlegadaMaterial edicion = new EdicionLlegadaMaterial();
            Patio patio= new Patio();
            patio.PatioID="1";
            patio.Nombre="Plaza Real";

            Proyecto proyecto= new Proyecto();
            proyecto.ProyectoID="1";
            proyecto.Nombre = "Proyecto Default";

            Files files = new Files();
            files.id = 1;
            files.Descripcion = "ARCHIVO 1";
            files.Archivo="documento";
            files.Extension = ".doc";
            lstfiles.Add(files);
            
            Files files1= new Files();
            files1.id = 1;
            files1.Descripcion = "ARCHIVO 2";
            files1.Archivo="documento2";
            files1.Extension = ".docx";
            lstfiles.Add(files1);


            Files files2 = new Files();
            files2.id = 1;
            files2.Descripcion = "ARCHIVO Pase Salida 1";
            files2.Archivo = "Doc 1";
            files2.Extension = ".doc";
            files2.TipoArchivo = "Pedimento";
            lstfiles2.Add(files2);

            Files files3 = new Files();
            files3.id = 1;
            files3.Descripcion = "ARCHIVO Pase Salida 2";
            files3.Archivo = "Doc 2";
            files3.Extension = ".docx";
            files3.TipoArchivo = "Factura";
            lstfiles2.Add(files3);

            Proveedor proveedor = new Proveedor();
            proveedor.ProveedorID = "1";
            proveedor.Nombre = "Proveedor Default";


            edicion.FolioAvisoLlegadaID = 123;
            edicion.FolioLlegadaID = 152;
            edicion.Proyecto = proyecto;
            edicion.Patio = patio;
            edicion.Archivos = lstfiles;
            edicion.Estatus = "En Patio";
            edicion.Proveedor = proveedor;
            edicion.OrdenCompra = "ODC001";
            edicion.Factura = "F00125";
            edicion.ArchivosPaseSalida = lstfiles2;
            return edicion;
        }

        // GET api/entradamaterial/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/entradamaterial
        public void Post([FromBody]string value)
        {
        }

        // PUT api/entradamaterial/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/entradamaterial/5
        public void Delete(int id)
        {
        }
    }
}
