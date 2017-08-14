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
    public class DummyAlmacenajeRackController : ApiController
    {
        public IEnumerable<AlmacenajeRack> Get(string token)
        {
            List<AlmacenajeRack> Lstalmacenaje = new List<AlmacenajeRack>();
            AlmacenajeRack almacenaje = new AlmacenajeRack();
            almacenaje.ItemCode = "1001";
            almacenaje.NumeroUnicoID = "1";
            almacenaje.NumeroUnico = "B-0001";
            almacenaje.Rack = "REF-1";
            almacenaje.TieneLocalizacion = "true";
            Lstalmacenaje.Add(almacenaje);

            AlmacenajeRack almacenaje1 = new AlmacenajeRack();
            almacenaje1.ItemCode = "1001";
            almacenaje1.NumeroUnicoID = "2";
            almacenaje1.NumeroUnico = "B-0002";
            almacenaje1.Rack = "REF-1";
            almacenaje1.TieneLocalizacion = "false";
            Lstalmacenaje.Add(almacenaje1);

            AlmacenajeRack almacenaje2 = new AlmacenajeRack();
            almacenaje2.ItemCode = "1001";
            almacenaje2.NumeroUnicoID = "3";
            almacenaje2.NumeroUnico = "B-0003";
            almacenaje2.Rack = "";
            almacenaje2.TieneLocalizacion = "true";
            Lstalmacenaje.Add(almacenaje2);


            AlmacenajeRack almacenaje3 = new AlmacenajeRack();
            almacenaje3.ItemCode = "1002";
            almacenaje3.NumeroUnicoID = "1";
            almacenaje3.NumeroUnico = "C-0001";
            almacenaje3.Rack = "";
            almacenaje3.TieneLocalizacion = "true";
            Lstalmacenaje.Add(almacenaje3);

            return Lstalmacenaje.AsEnumerable();
        }

    }
}
