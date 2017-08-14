using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.LlegadaMaterial
{
    public class EntradaMaterialController : Controller
    {
        //
        // GET: /EntradaMaterial/
        public ActionResult DetalleLlegadaMaterial()
        {
            return View();
        }

        // GET: /EntradaMaterial/ListadoLlegadaMaterial
        public ActionResult ListadoLlegadaMaterial()
        {
            return View();
        }
        //
        // GET: /EntradaMaterial/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /EntradaMaterial/DashboardLlegadaMaterial
        public ActionResult DashboardLlegadaMaterial()
        {
            return View();
        }

        //
        // GET: /EntradaMaterial/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PaseDeSalida()
        {
            return View();
        }

        public ActionResult FormatoPaseSalida()
        {
            return View();
        }

        public ActionResult FormatoIncidencias()
        {
            return View();
        }
    }
}
