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
    public class DummyGridOrdenRecepcionController : ApiController
    {
        // GET api/dummygridordenrecepcion
        public IEnumerable<DummyGridOrdenRecepcion> Get(int proyectoID,int folioavisoentradaID,int ItemCodeID, string token)
        {
            List<DummyGridOrdenRecepcion> lstGrid = new List<DummyGridOrdenRecepcion>();
            DummyGridOrdenRecepcion row1 = new DummyGridOrdenRecepcion();
            DummyGridOrdenRecepcion row2 = new DummyGridOrdenRecepcion();
            DummyGridOrdenRecepcion row3 = new DummyGridOrdenRecepcion();

            List<DummyMaterial> lstmat1 = new List<DummyMaterial>();
            DummyMaterial mat1= new DummyMaterial();
            mat1.ItemCodeID = 3;
            mat1.Codigo = "432";
            mat1.Descripcion = "Mat1";
            mat1.D1 = 10;
            mat1.D2 = 11;
            mat1.Cantidad = 12;
            mat1.TipoMaterial = "Accesorio";
            lstmat1.Add(mat1);

            List<DummyMaterial> lstmat3 = new List<DummyMaterial>();
            DummyMaterial mat3 = new DummyMaterial();
            mat3.ItemCodeID = 4;
            mat3.Codigo = "433";
            mat3.Descripcion = "Mat3";
            mat3.D1 = 21;
            mat3.D2 = 22;
            mat3.Cantidad = 23;
            mat3.TipoMaterial = "Tubos";
            lstmat3.Add(mat3);

            List<DummyMaterial> lstmat2 = new List<DummyMaterial>();
            DummyMaterial mat2 = new DummyMaterial();
            mat2.ItemCodeID = 4;
            mat2.Codigo = "510";
            mat2.Descripcion = "Mat2";
            mat2.D1 = 15;
            mat2.D2 = 16;
            mat2.Cantidad = 21;
            mat2.TipoMaterial = "Accesorio";
            lstmat2.Add(mat2);


            row1.AvisoEntradaID = 1010;
            row1.Accesorios = lstmat1;
            row1.Tubos = lstmat3;

            row2.AvisoEntradaID = 1020;
            row2.Accesorios = lstmat2;

            lstGrid.Add(row1);
            lstGrid.Add(row2);
            return lstGrid.AsEnumerable();
        }

        // GET api/dummygridordenrecepcion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummygridordenrecepcion
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummygridordenrecepcion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummygridordenrecepcion/5
        public void Delete(int id)
        {
        }
    }
}
