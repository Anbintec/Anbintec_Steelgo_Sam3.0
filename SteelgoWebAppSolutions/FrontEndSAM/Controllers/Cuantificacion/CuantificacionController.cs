using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.Cuantificacion
{
    public class CuantificacionController : Controller
    {
        //
        // GET: /Cuantificacion/
        public ActionResult Cuantificacion()
        {
            return View();
        }
        public ActionResult ListadoPackingList()
        {
            return View();
        }
        //
        // GET: /Cuantificacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Cuantificacion/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cuantificacion/Create
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
        // GET: /Cuantificacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Cuantificacion/Edit/5
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
        // GET: /Cuantificacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Cuantificacion/Delete/5
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
