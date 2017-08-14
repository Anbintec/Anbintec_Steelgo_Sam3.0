using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndSAM.Controllers.PackingList
{
    public class PackingListController : Controller
    {
        //
        // GET: /ListadoPackingList/
        public ActionResult ListadoPackingList()
        {
            return View();
        }

        //
        // GET: /ListadoPackingList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ListadoPackingList/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ListadoPackingList/Create
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
        // GET: /ListadoPackingList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ListadoPackingList/Edit/5
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
        // GET: /ListadoPackingList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ListadoPackingList/Delete/5
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
