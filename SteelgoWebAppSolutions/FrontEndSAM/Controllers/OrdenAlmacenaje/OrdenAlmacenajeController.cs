using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.OrdenAlmacenaje
{
    public class OrdenAlmacenajeController : Controller
    {
        //
        // GET: /OrdenAlmacenaje/
        public ActionResult ListadoOrdenAlmacenaje()
        {
            return View();
        }

        public ActionResult GenerarOrdenAlmacenaje()
        {
            return View();
        }

        public ActionResult DetalleOrdenAlmacenaje()
        {
            return View();
        }

        //
        // GET: /OrdenAlmacenaje/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /OrdenAlmacenaje/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OrdenAlmacenaje/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OrdenAlmacenaje/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OrdenAlmacenaje/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OrdenAlmacenaje/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OrdenAlmacenaje/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
