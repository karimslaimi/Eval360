using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class AxeController : Controller
    {
        ApplicationDbContext db;
        public AxeController(ApplicationDbContext db)
        {
            this.db = db;
        }





        // GET: AxeController
        public ActionResult Index()
        {
            return View(this.db.AxeEval.ToArray());
        }

        // GET: AxeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AxeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AxeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AxeEval axeEval)
        {
            if (ModelState.IsValid)
            {
                db.Add(axeEval);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(axeEval);
        }

        // GET: AxeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AxeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AxeEval axeEval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(axeEval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(axeEval);
        }

        
        public ActionResult Delete(int id)
        {
            var axeEval = db.AxeEval.Find(id);
            if (axeEval == null)
            {
                return NotFound();
            }
            this.db.AxeEval.Remove(axeEval);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

      
    }
}
