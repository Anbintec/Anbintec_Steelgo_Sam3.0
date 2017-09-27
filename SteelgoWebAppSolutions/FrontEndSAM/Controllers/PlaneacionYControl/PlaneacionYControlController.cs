using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FrontEndSAM.Controllers.PlaneacionYControl
{
    public class PlaneacionYControlController : Controller
    {
        //
        // GET: /EmisionOT/
        public ActionResult EmisionOT()
        {
            return View();
        }

        public ActionResult OrdenesDeTrabajo(string Proyecciones)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //Talleres capturaDatosJson = serializer.Deserialize<Talleres>(Talleres);

            ViewBag.Proyecciones = Proyecciones.ToString();
            return View();

        }

        public ActionResult Ciclos()
        {
            return View();
        }

        public ActionResult Peticion()
        {
            return View();
        }
        public ActionResult PeticionMontaje()
        {
            return View();
        }
        public ActionResult AutorizacionPeticion()
        {
            return View();
        }
        public ActionResult Capacidad()
        {
            return View();
        }
    }
}