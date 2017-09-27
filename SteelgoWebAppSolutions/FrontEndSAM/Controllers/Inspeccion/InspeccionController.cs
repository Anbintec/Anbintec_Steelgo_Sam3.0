using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Inspeccion
{
    public class InspeccionController : Controller
    {
        public ActionResult InspeccionDimensional()
        {
            return View();
        }

        public ActionResult InspeccionVisualDimensional()
        {
            return View();
        }
    }
}
