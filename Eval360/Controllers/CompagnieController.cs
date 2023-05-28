using Eval360.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    public class CompagnieController : Controller
    {
        ApplicationDbContext db;

        public CompagnieController(ApplicationDbContext db)
        {
            this.db = db;
        }





        // GET: CompagnieController
        public ActionResult Index()
        {
            return View(this.db.Compagnie.Include(x=>x.employee).ToArray());
        }

        // GET: CompagnieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompagnieController/Create
        public ActionResult Create()
        {
            ViewBag.employeeList = this.db.User.Where(x=>x.LockoutEnabled && x.LockoutEnd>DateTime.Now).ToArray();
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View();
        }

        // POST: CompagnieController/Create
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

        // GET: CompagnieController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompagnieController/Edit/5
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

        // GET: CompagnieController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompagnieController/Delete/5
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
