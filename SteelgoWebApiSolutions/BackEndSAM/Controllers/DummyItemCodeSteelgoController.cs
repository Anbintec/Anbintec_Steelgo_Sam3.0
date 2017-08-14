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
    public class DummyItemCodeSteelgoController : ApiController
    {
        // GET api/dummyitemcodesteelgo
        public IEnumerable<ItemCode> Get( string token)
        {
            List<ItemCode> lstItemCode = new List<ItemCode>();
            ItemCode itemCode = new ItemCode();
            ItemCode itemCode1 = new ItemCode();
            ItemCode itemCode2 = new ItemCode();
            ItemCode itemCode3 = new ItemCode();
            ItemCode itemCode4 = new ItemCode();

            itemCode4.ItemCodeID = "-1";
            itemCode4.Codigo = "Agregar Nuevo";
            lstItemCode.Add(itemCode4);

            itemCode.ItemCodeID = "90";
            itemCode.Codigo = "P90111";
            itemCode.Familia = "FAM 1";
            itemCode.Cedula = "CED 1";
            itemCode.TipoAcero = "TIPOACERO 1";
            lstItemCode.Add(itemCode);

            itemCode1.ItemCodeID = "91";
            itemCode1.Codigo = "P90112";
            itemCode1.Familia = "FAM 2";
            itemCode1.Cedula = "CED 2";
            itemCode1.TipoAcero = "TIPOACERO 2";
            lstItemCode.Add(itemCode1);

            itemCode2.ItemCodeID = "92";
            itemCode2.Codigo = "P90113";
            itemCode2.Familia = "FAM 3";
            itemCode2.Cedula = "CED 3";
            itemCode2.TipoAcero = "TIPOACERO 2";
            lstItemCode.Add(itemCode2);

            return lstItemCode.AsEnumerable();
        }

        // GET api/dummyitemcodesteelgo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyitemcodesteelgo
        public void Post(string data, string token)
        {
        }

        // PUT api/dummyitemcodesteelgo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyitemcodesteelgo/5
        public void Delete(int id)
        {
        }
    }
}
