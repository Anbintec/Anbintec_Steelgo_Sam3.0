using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.ListadoMateriales
{
    public class ListadoMaterialesController : Controller
    {
        //
        // GET: /ListadoMateriales/
        public ActionResult ListadoMateriales()
        {
            return View();
        }

        public ActionResult DetalleNumeroUnico()
        {
            return View();
        }

        public ActionResult ListadoMaterialesItemCode()
        {
            return View();
        }
    }
}
