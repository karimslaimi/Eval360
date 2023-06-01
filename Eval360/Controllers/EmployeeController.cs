using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eval360.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;


        public EmployeeController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult myEval()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;
            var evals = this.db.Compagnie.Where(x => x.userId == currentUser.Id).ToList() ;
            return View(evals);
        }
    }
}
