using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Controllers
{
    public class TextController : Controller
    {
        // GET: TextController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TextController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TextController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TextController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TextController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TextController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TextController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
