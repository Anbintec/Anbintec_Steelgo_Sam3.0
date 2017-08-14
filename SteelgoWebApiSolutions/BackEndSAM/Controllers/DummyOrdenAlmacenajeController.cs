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
    public class DummyOrdenAlmacenajeController : ApiController
    {
        // GET api/dummyordenalmacenaje
        public IEnumerable<OrdenAlmacenajeJson> Get(string data)
        {
            List<OrdenAlmacenajeJson> lstOrdenAlmacenaje = new List<OrdenAlmacenajeJson>();
            OrdenAlmacenajeJson OrdenAlmacenaje1 = new OrdenAlmacenajeJson();
            OrdenAlmacenajeJson OrdenAlmacenaje2 = new OrdenAlmacenajeJson();

            List<PackingListCuantificacion> lstPackingList = new List<PackingListCuantificacion>();
            PackingListCuantificacion packingList1 = new PackingListCuantificacion();
            PackingListCuantificacion packingList2 = new PackingListCuantificacion();

            List<ElementoItemCodeGenerarOrdenAlmacenaje> lstitemcodes= new List<ElementoItemCodeGenerarOrdenAlmacenaje>();
            ElementoItemCodeGenerarOrdenAlmacenaje Item1= new ElementoItemCodeGenerarOrdenAlmacenaje();
            ElementoItemCodeGenerarOrdenAlmacenaje Item2= new ElementoItemCodeGenerarOrdenAlmacenaje();

            Item1.OrdenAlmacenaje = "10";
            //Item1.FolioCuantificacion = "PL001";
            Item1.ItemCodeID="A1";
            Item1.Codigo="A1";
            Item1.NumeroUnico="BK001";
            lstitemcodes.Add(Item1);

            Item2.OrdenAlmacenaje = "10";
            //Item2.FolioCuantificacion = "PL001";
            Item2.ItemCodeID="A2";
            Item2.Codigo="A2";
            Item2.NumeroUnico="BK002";
            lstitemcodes.Add(Item2);

            packingList1.OrdenAlmacenaje = "10";
            packingList1.FolioCuantificacion = "PL001";
            packingList1.ItemCodes = lstitemcodes;
            lstPackingList.Add(packingList1);


            List<ElementoItemCodeGenerarOrdenAlmacenaje> lstitemcodes1 = new List<ElementoItemCodeGenerarOrdenAlmacenaje>();
            ElementoItemCodeGenerarOrdenAlmacenaje Item3 = new ElementoItemCodeGenerarOrdenAlmacenaje();
            ElementoItemCodeGenerarOrdenAlmacenaje Item4 = new ElementoItemCodeGenerarOrdenAlmacenaje();

            Item3.OrdenAlmacenaje = "10";
            //Item3.FolioCuantificacion = "PL002";
            Item3.ItemCodeID = "A3";
            Item3.Codigo = "A3";
            Item3.NumeroUnico = "BK003";
            lstitemcodes1.Add(Item3);

            Item4.OrdenAlmacenaje = "10";
            //Item4.FolioCuantificacion = "PL002";
            Item4.ItemCodeID = "A4";
            Item4.Codigo = "A4";
            Item4.NumeroUnico = "BK004";
            lstitemcodes1.Add(Item4);

            packingList2.OrdenAlmacenaje = "10";
            packingList2.FolioCuantificacion = "PL002";
            packingList2.ItemCodes = lstitemcodes1;
            lstPackingList.Add(packingList2);

            OrdenAlmacenaje1.FechaOrdenAlmacenaje = "28/09/1987";
            OrdenAlmacenaje1.OrdenAlmacenaje = "10";
            OrdenAlmacenaje1.FolioCuantificacion = lstPackingList;
            lstOrdenAlmacenaje.Add(OrdenAlmacenaje1);


            List<PackingListCuantificacion> lstPackingList2 = new List<PackingListCuantificacion>();
            PackingListCuantificacion packingList3 = new PackingListCuantificacion();
            PackingListCuantificacion packingList4 = new PackingListCuantificacion();

            List<ElementoItemCodeGenerarOrdenAlmacenaje> lstitemcodes2 = new List<ElementoItemCodeGenerarOrdenAlmacenaje>();
            ElementoItemCodeGenerarOrdenAlmacenaje Item5 = new ElementoItemCodeGenerarOrdenAlmacenaje();
            ElementoItemCodeGenerarOrdenAlmacenaje Item6 = new ElementoItemCodeGenerarOrdenAlmacenaje();

            Item5.OrdenAlmacenaje = "11";
            //Item5.FolioCuantificacion = "PL003";
            Item5.ItemCodeID = "A5";
            Item5.Codigo = "A5";
            Item5.NumeroUnico = "BK005";
            lstitemcodes2.Add(Item5);

            Item6.OrdenAlmacenaje = "11";
            //Item6.FolioCuantificacion = "PL003";
            Item6.ItemCodeID = "A6";
            Item6.Codigo = "A6";
            Item6.NumeroUnico = "BK006";
            lstitemcodes2.Add(Item6);
            
            packingList3.OrdenAlmacenaje = "11";
            packingList3.FolioCuantificacion = "PL003";
            packingList3.ItemCodes = lstitemcodes2;
            lstPackingList2.Add(packingList3);

            OrdenAlmacenaje2.FechaOrdenAlmacenaje = "30/09/2001";
            OrdenAlmacenaje2.OrdenAlmacenaje = "11";
            OrdenAlmacenaje2.FolioCuantificacion = lstPackingList2;
            lstOrdenAlmacenaje.Add(OrdenAlmacenaje2);

            return lstOrdenAlmacenaje.AsEnumerable();
        }

        // GET api/dummyordenalmacenaje/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyordenalmacenaje
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyordenalmacenaje/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyordenalmacenaje/5
        public void Delete(int id)
        {
        }
    }
}
