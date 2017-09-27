using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.ConfiguracionSoldadura
{
    public class ConfiguracionSoldaduraController : Controller
    {
        public ActionResult PQR()
        {
            return View();
        }

        public ActionResult WPS()
        {
            return View();
        }
        public ActionResult SoldadorCertificacion()
        {
            return View();
        }

        public ActionResult NuevoPQR()
        {
            return View();
        }

        public ActionResult CrearWPS()
        {
            return View();
        }
        public ActionResult NuevoSoldadorCertificacion()
        {
            return View();
        }
    }
}
