using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Materiales
{
    public class MaterialesController : Controller
    {
        public ActionResult NotificacionDeficit()
        {
            return View();
        }

        public ActionResult RevisionDeficit()
        {
            return View();
        }
    }
}