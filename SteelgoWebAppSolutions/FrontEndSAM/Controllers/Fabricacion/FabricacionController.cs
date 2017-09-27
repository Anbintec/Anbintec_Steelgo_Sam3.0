using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Fabricacion
{
    public class FabricacionController : Controller
    {
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
