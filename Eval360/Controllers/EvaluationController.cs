﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eval360.Controllers
{
    public class EvaluationController : Controller
    {
        // GET: EvaluationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EvaluationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EvaluationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EvaluationController/Create
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

        // GET: EvaluationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EvaluationController/Edit/5
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

        // GET: EvaluationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EvaluationController/Delete/5
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
