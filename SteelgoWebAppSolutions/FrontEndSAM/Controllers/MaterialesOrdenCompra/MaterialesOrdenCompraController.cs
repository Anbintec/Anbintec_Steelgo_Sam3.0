using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.MaterialesOrdenCompra
{
    public class MaterialesOrdenCompraController : Controller
    {
        // GET: MaterialesOrdenCompra
        // GET: Dynasol
        public ActionResult OrdenesCompra()
        {
            return View();
        }
        public ActionResult PackingList()
        {
            return View();
        }
        public ActionResult Camion()
        {
            return View();
        }
        public ActionResult CompraPagoOC()
        {
            return View();
        }
        public ActionResult RecepcionOC()
        {
            return View();
        }
        public ActionResult CobroOC()
        {
            return View();
        }
        public ActionResult PagoOC()
        {
            return View();
        }
        public ActionResult CreacionOC()
        {
            return View();
        }
    }
}