using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class DirectionController : Controller
    {
        ApplicationDbContext db;

        public DirectionController(ApplicationDbContext context)
        {
            this.db = context;

        }

        public IActionResult Index()
        {
           
            return View(this.db.Directions.ToArray());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Direction());

        }
        [HttpPost]
        public IActionResult Create(Direction direction)
        {
            if (ModelState.IsValid)
            {
                db.Add(direction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(direction);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            return View(this.db.Directions.Find(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Direction direction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(direction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(direction);

        }

        public IActionResult Delete(int id)
        {
            var direction = db.Directions.Find(id);
            if (direction == null)
            {
                return NotFound();
            }
            this.db.Directions.Remove(direction);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
