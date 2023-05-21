﻿using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    public class PosteController : Controller
    {
        ApplicationDbContext db;

        public PosteController(ApplicationDbContext db)
        {
            this.db = db;
        }




        // GET: PosteController
        public ActionResult Index()
        {
            return View(this.db.Poste.Include(x=>x.Direction).Include(u=>u.users).ToArray());
        }

        // GET: PosteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PosteController/Create
        public ActionResult Create()
        {
            ViewBag.DirectionsList = new SelectList(this.db.Directions.ToArray(), "id", "name");

            return View(new Poste());
        }

        // POST: PosteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Poste poste)
        {
            if (ModelState.IsValid)
            {
                db.Add(poste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(poste);

        }

        // GET: PosteController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            ViewBag.DirectionsList = new SelectList(this.db.Directions.ToArray(), "id", "name");

            return View(this.db.Poste.Find(id));
        }

        // POST: PosteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Poste poste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(poste);
        }

        public IActionResult Delete(int id)
        {
            var poste = db.Poste.Find(id);
            if (poste == null)
            {
                return NotFound();
            }
            this.db.Poste.Remove(poste);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}