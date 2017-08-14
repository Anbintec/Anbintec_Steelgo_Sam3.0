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
    public class DummyObtenerItemCodesController : ApiController
    {
        // GET api/dummyobteneritemcodes
        public IEnumerable<ListadoItemCodes> Get(string itemcodes,string username, string token)
        {
            List<ListadoItemCodes> lstItemCodes = new List<ListadoItemCodes>();
            ListadoItemCodes itemCodes = new ListadoItemCodes();
            ListadoItemCodes itemCodes1 = new ListadoItemCodes();
            ItemCode _ITEM = new ItemCode();
            ItemCode _ITEM1 = new ItemCode();
            Proyecto proyecto = new Proyecto();
            Proyecto proyecto1 = new Proyecto();

            proyecto.ProyectoID="1";
            proyecto.Nombre="PRUEBA 1";

            _ITEM.ItemCodeID = "1223434";
            _ITEM.Codigo = "AAC957927OOY";
            _ITEM.Descripcion = "Brida";
            _ITEM.Familia = "Brida";
            _ITEM.Cedula = "STD";
            _ITEM.TipoAcero = "CS";
            _ITEM.D1 = 10;
            _ITEM.D2 = 2;
            _ITEM.Cantidad = 3;
            _ITEM.Colada="COLADA";
            _ITEM.Certificado="CERTIFICADO";
            _ITEM.Material="MATERIAL";
            _ITEM.NumeroUnico = "NUMEROUNICO";
            

            itemCodes.Proyecto = proyecto;
            itemCodes.ItemCode = _ITEM;
            lstItemCodes.Add(itemCodes);


            proyecto1.ProyectoID = "2";
            proyecto1.Nombre = "PRUEBA 2";

            _ITEM1.ItemCodeID = "155555";
            _ITEM1.Codigo = "BBBBBBBBBBB";
            _ITEM1.Descripcion = "AAAA";
            _ITEM1.Familia = "AAAAA";
            _ITEM1.Cedula = "A";
            _ITEM1.TipoAcero = "A";
            _ITEM1.D1 = 10;
            _ITEM1.D2 = 2;
            _ITEM1.Cantidad = 3;
            _ITEM1.Colada = "COLADA1";
            _ITEM1.Certificado = "CERTIFICADO1";
            _ITEM1.Material = "MATERIAL1";
            _ITEM1.NumeroUnico = "NUMEROUNICO1";


            itemCodes1.Proyecto = proyecto1;
            itemCodes1.ItemCode = _ITEM1;
            lstItemCodes.Add(itemCodes1);

            return lstItemCodes.AsEnumerable();
        }

        // GET api/dummyobteneritemcodes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyobteneritemcodes
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyobteneritemcodes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyobteneritemcodes/5
        public void Delete(int id)
        {
        }
    }
}
