using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Produccion
{
    public class ProduccionController : Controller
    {
        // GET: Produccion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Armado()
        {
            return View();
        }

        public ActionResult Soldadura()
        {
            return View();
        }
    }
}