using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Incidencias
{
    public class IncidenciasController : Controller
    {
        //
        // GET: /Incidencias/
        public ActionResult Incidencias()
        {
            return View();
        }

        public ActionResult ListadoIncidencias()
        {
            return View();
        }
    }
}
