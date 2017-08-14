using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyGenerarOrdenAlmacenajeController : ApiController
    {
        //// GET api/dummygenerarordenalmacenaje
        //public IEnumerable<DummyFolioCuantificacion> Get(string data)
        //{
        //    List<DummyFolioCuantificacion> lstFolioCuantificacion1= new List<DummyFolioCuantificacion>();
        //    DummyFolioCuantificacion DummyFolioCuantificacion1=new DummyFolioCuantificacion();
        //    DummyFolioCuantificacion DummyFolioCuantificacion2=new DummyFolioCuantificacion();

        //    List<ElementoCuantificacionItemCode> lstItemCodes1 = new List<ElementoCuantificacionItemCode>();
        //    ElementoCuantificacionItemCode itemcode1 = new ElementoCuantificacionItemCode();
        //    ElementoCuantificacionItemCode itemcode2 = new ElementoCuantificacionItemCode();

        //List<ElementoNumeroUnico> lstNumeros = new List<ElementoNumeroUnico>();
        //    ElementoNumeroUnico numero1 = new ElementoNumeroUnico();
        //    ElementoNumeroUnico numero2 = new ElementoNumeroUnico();
        //    ElementoNumeroUnico numero3 = new ElementoNumeroUnico();

        //    numero1.FolioCuantificacion = "10";
        //    numero1.ItemCodeID = "IT0001";
        //    numero1.NumeroUnicoID = "BK0001";
        //    numero1.NumeroUnico = "NUMBER1";
        //    lstNumeros.Add(numero1);

        //    numero2.FolioCuantificacion = "10";
        //    numero2.ItemCodeID = "IT0001";
        //    numero2.NumeroUnicoID = "BK0002";
        //    numero2.NumeroUnico = "NUMBER2";
        //    lstNumeros.Add(numero2);

        //    numero3.FolioCuantificacion = "10";
        //    numero3.ItemCodeID = "IT0001";
        //    numero3.NumeroUnicoID = "BK0003";
        //    numero3.NumeroUnico = "NUMBER3";
        //    lstNumeros.Add(numero3);

        //    itemcode1.FolioCuantificacion = "10";
        //    itemcode1.ItemCodeID = "IT0001";
        //    itemcode1.Codigo = "IT0001";
        //    itemcode1.Descripcion = "Item Code 1";
        //    itemcode1.D1 = "2";
        //    itemcode1.D2 = "0";
        //    itemcode1.Cantidad = "3";
        //    itemcode1.NumerosUnicos = lstNumeros;
        //    lstItemCodes1.Add(itemcode1);


        //    List<ElementoNumeroUnico> lstNumeros2 = new List<ElementoNumeroUnico>();
        //    ElementoNumeroUnico numero4 = new ElementoNumeroUnico();
        //    ElementoNumeroUnico numero5 = new ElementoNumeroUnico();

        //    numero4.FolioCuantificacion = "10";
        //    numero4.ItemCodeID = "IT0002";
        //    numero4.NumeroUnicoID = "BK0004";
        //    numero4.NumeroUnico = "NUMBER4";
        //    lstNumeros2.Add(numero4);

        //    numero5.FolioCuantificacion = "10";
        //    numero5.ItemCodeID = "IT0002";
        //    numero5.NumeroUnicoID = "BK0005";
        //    numero5.NumeroUnico = "NUMBER5";
        //    lstNumeros2.Add(numero5);

        //    itemcode2.FolioCuantificacion = "10";
        //    itemcode2.ItemCodeID = "IT0002";
        //    itemcode2.Codigo = "IT0002";
        //    itemcode2.Descripcion = "Item Code 2";
        //    itemcode2.D1 = "3";
        //    itemcode2.D2 = "0";
        //    itemcode2.Cantidad = "2";
        //    itemcode2.NumerosUnicos = lstNumeros2;
        //    lstItemCodes1.Add(itemcode2);

        //    DummyFolioCuantificacion1.FolioCuantificacion = "10";
        //    DummyFolioCuantificacion1.ItemCodes = lstItemCodes1;


        //    List<ElementoCuantificacionItemCode> lstItemCodes2 = new List<ElementoCuantificacionItemCode>();
        //    ElementoCuantificacionItemCode itemcode3 = new ElementoCuantificacionItemCode();
        //    ElementoCuantificacionItemCode itemcode4 = new ElementoCuantificacionItemCode();

        //    List<ElementoNumeroUnico> lstNumeros6 = new List<ElementoNumeroUnico>();
        //    ElementoNumeroUnico numero6 = new ElementoNumeroUnico();
        //    ElementoNumeroUnico numero7 = new ElementoNumeroUnico();

        //    numero6.FolioCuantificacion = "11";
        //    numero6.ItemCodeID = "IT0001";
        //    numero6.NumeroUnicoID = "BK0006";
        //    numero6.NumeroUnico = "NUMBER6";
        //    lstNumeros6.Add(numero6);

        //    numero7.FolioCuantificacion = "11";
        //    numero7.ItemCodeID = "IT0001";
        //    numero7.NumeroUnicoID = "BK0007";
        //    numero7.NumeroUnico = "NUMBER7";
        //    lstNumeros6.Add(numero7);

        //    itemcode3.FolioCuantificacion = "11";
        //    itemcode3.ItemCodeID = "IT0001";
        //    itemcode3.Codigo = "IT0001";
        //    itemcode3.Descripcion = "Item Code SS0001";
        //    itemcode3.D1 = "2";
        //    itemcode3.D2 = "0";
        //    itemcode3.Cantidad = "2";
        //    itemcode3.NumerosUnicos = lstNumeros6;
        //    lstItemCodes2.Add(itemcode3);

        //    DummyFolioCuantificacion2.FolioCuantificacion = "11";
        //    DummyFolioCuantificacion2.ItemCodes = lstItemCodes2;

        //    lstFolioCuantificacion1.Add(DummyFolioCuantificacion1);
        //    lstFolioCuantificacion1.Add(DummyFolioCuantificacion2);

        //    return lstFolioCuantificacion1.AsEnumerable();
        //}

        // GET api/dummygenerarordenalmacenaje/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummygenerarordenalmacenaje
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummygenerarordenalmacenaje/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummygenerarordenalmacenaje/5
        public void Delete(int id)
        {
        }
    }
}
