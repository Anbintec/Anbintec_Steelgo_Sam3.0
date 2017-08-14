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
    public class DummyCargarGridPackingListController : ApiController
    {
        // GET api/dummycargargridpackinglist
        public GridCuantificacion Get(string gridPackingList, string username, string token)
        {
            
            GridCuantificacion GridCuantificacion = new GridCuantificacion();
            List<ListadoCuantificacion> lstCuantificacion= new List<ListadoCuantificacion>();
            ListadoCuantificacion cuantificacion = new ListadoCuantificacion();
            PackingList packingList = new PackingList();
            ListadoCuantificacion cuantificacion1 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion2 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion3 = new ListadoCuantificacion();

            cuantificacion.ItemCode = "AAC957927OOY";
            cuantificacion.Descripcion = "Brida";
            cuantificacion.D1 = 10;
            cuantificacion.D2 = 2;
            cuantificacion.ItemCodeSteelgo = "AAC957927OOY-1";
            cuantificacion.Familia = "Brida";
            cuantificacion.Cedula = "STD";
            cuantificacion.TipoAcero = "CS";
            cuantificacion.Colada = "Colada1";
            cuantificacion.Cantidad = 3;
            cuantificacion.MM = 11;
            cuantificacion.Detallar = "No";
            cuantificacion.TieneNU = true;
            cuantificacion.TieneError = false;
            lstCuantificacion.Add(cuantificacion);

            cuantificacion1.ItemCode = "AAC957927OOZ";
            cuantificacion1.Descripcion = "Sockolet";
            cuantificacion1.D1 = 8;
            cuantificacion1.D2 = 0;
            cuantificacion1.ItemCodeSteelgo = "AAC957927OOZ-2";
            cuantificacion1.Familia = "";
            cuantificacion1.Cedula = "";
            cuantificacion1.TipoAcero = "";
            cuantificacion1.Colada = "";
            cuantificacion1.Cantidad = 150;
            cuantificacion1.MM = 151;
            cuantificacion1.Detallar = "No";
            cuantificacion1.TieneNU = false;
            cuantificacion1.TieneError = false;
            lstCuantificacion.Add(cuantificacion1);

            cuantificacion2.ItemCode = "AAC957927OOX";
            cuantificacion2.Descripcion = "Codo45";
            cuantificacion2.D1 = 10;
            cuantificacion2.D2 = 0;
            cuantificacion2.ItemCodeSteelgo = "AAC957927OOX-3";
            cuantificacion2.Familia = "Codo";
            cuantificacion2.Cedula = "STD";
            cuantificacion2.TipoAcero = "SS";
            cuantificacion2.Colada = "SS1";
            cuantificacion2.Cantidad = 1;
            cuantificacion2.MM = 2;
            cuantificacion2.Detallar = "No";
            cuantificacion2.TieneNU = true;
            cuantificacion2.TieneError = false;
            lstCuantificacion.Add(cuantificacion2);


            cuantificacion3.ItemCode = "AAC957927OOT";
            cuantificacion3.Descripcion = "ARNIKA";
            cuantificacion3.D1 = 9;
            cuantificacion3.D2 = 1;
            cuantificacion3.ItemCodeSteelgo = "AAC957927OOT-4";
            cuantificacion3.Familia = "ALTA";
            cuantificacion3.Cedula = "ASDA";
            cuantificacion3.TipoAcero = "ABRICA";
            cuantificacion3.Colada = "a1";
            cuantificacion3.Cantidad = 160;
            cuantificacion3.MM = 161;
            cuantificacion3.Detallar = "No";
            cuantificacion3.TieneNU = false;
            cuantificacion3.TieneError = false;
            lstCuantificacion.Add(cuantificacion3);

            packingList.PackingListID = "900";
            packingList.Consecutivo = "1";

            GridCuantificacion.PackingList = packingList;
            GridCuantificacion.ListadoCuantificacion = lstCuantificacion;
            return GridCuantificacion;
        }

        // GET api/dummycargargridpackinglist/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummycargargridpackinglist
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummycargargridpackinglist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummycargargridpackinglist/5
        public void Delete(int id)
        {
        }
    }
}
