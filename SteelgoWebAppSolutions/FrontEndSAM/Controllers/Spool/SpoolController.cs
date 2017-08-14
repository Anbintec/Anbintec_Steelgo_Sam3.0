using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Spool
{
    public class SpoolController : Controller
    {
        public ActionResult ListadoSpoolGranel() 
        {
            return View();        
        }

        public ActionResult SpoolGranel()
        {
            return View();
        }
    }
}