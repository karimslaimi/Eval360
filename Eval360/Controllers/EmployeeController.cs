using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    [CustomAuthorization(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;
        private User currentUser;


        public EmployeeController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult myEval()
        {
            var evals = this.db.Compagnie.Where(x => x.userId == this.currentUser.Id).Include(x=>x.compagnieQuestions).ToList() ;
            return View(evals);
        }

        public IActionResult toEval()
        {
            var evals = this.db.Compagnie.Where(x => x.compagnieUser.Select(u => u.userId).Contains(currentUser.Id)).ToArray();
            return View(evals);
        }

        public IActionResult historique()
        {
            var evals = from compagnie in this.db.Compagnie
                        join compagnieQuestion in this.db.CompagnieQuestions on compagnie.id equals compagnieQuestion.compagnieId
                        join compagnieResponse in this.db.CompagnieResponse on compagnieQuestion.id equals compagnieResponse.compagnieQuestionId
                        where compagnieResponse.userId == this.currentUser.Id select compagnie;

            return View(evals);
        }

        
    }
}
