using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eval360.Controllers
{
    public class CompagnieController : Controller
    {
        ApplicationDbContext db;
        private UserManager<User> userManager;

        public CompagnieController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.db = db;
        }





        // GET: CompagnieController
        public ActionResult Index()
        {
            var compagnies = this.db.Compagnie.Include(x => x.employee).Include(x=>x.compagnieQuestions).Include(x=>x.compagnieUser).ToArray();
            return View(compagnies) ;
        }

        // GET: CompagnieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompagnieController/Create
        public ActionResult Create()
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(new Compagnie());
        }

        // POST: CompagnieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compagnie compagnie, IFormCollection collection)
        {
            string questionsValue = collection["questions"];
            string usersValue = collection["users"];

            // Split the field value
            string[] questionsList = questionsValue.Split(',');
            string[] usersList = usersValue.Split(',');

            var questions = this.db.Question.Where(x => questionsList.Contains(x.id.ToString())).ToList();
            var users = this.db.Users.Where(x => usersList.Contains(x.Id)).ToList();
            ModelState.Remove("compagnieUser");
            ModelState.Remove("compagnieQuestions");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("employee.compagnieReponses");
            ModelState.Remove("employee.compagnieUser");
            ModelState.Remove("employee.compagnies");


            if (ModelState.IsValid)
            {
                compagnie.employee = this.userManager.FindByIdAsync(compagnie.employee.Id).Result;
                this.db.Compagnie.Add(compagnie);
                this.db.SaveChanges();
                
                this.AddQuestions(compagnie, questions);

                this.AddUsers(compagnie, users);

                return RedirectToAction("index");
            }

            var usersVB = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(usersVB.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(compagnie);
        }

        // GET: CompagnieController/Edit/5
        public ActionResult Edit(int id)
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            var compagnie = this.db.Compagnie.Where(x => x.id == id).Include(x => x.compagnieUser).Include(x=>x.compagnieQuestions).FirstOrDefault();
            return View(compagnie);
        }

        // POST: CompagnieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Compagnie compagnie, IFormCollection collection)
        {
            //check if the users have changed, the questions too

            string questionsValue = collection["questions"];
            string usersValue = collection["users"];

            // Split the field value
            string[] questionsList = questionsValue.Split(',');
            string[] usersList = usersValue.Split(',');
            //the list of fetched users and questions
            var questions = this.db.Question.Where(x => questionsList.Contains(x.id.ToString())).ToList();
            var users = this.db.Users.Where(x => usersList.Contains(x.Id)).ToList();
            //the current list of users and questions
            var compagnieQuestions = this.db.CompagnieQuestions.Where(x => x.compagnie.id == compagnie.id).Include(s=>s.question).Select(s=>s.question).ToList();
            var compagnieUsers = this.db.CompagnieUser.Where(x => x.compagnie.id == compagnie.id).Include(s => s.user).Select(s => s.user).ToList();

            var questionToAdd = this.questionToAdd(compagnieQuestions, questions);
            var questionToRemove = this.questionToRemove(compagnieQuestions, questions);

            var usersToRemove = this.usersToRemove(compagnieUsers, users);
            var userToAdd = this.userToAdd(compagnieUsers, users);

            ModelState.Remove("compagnieUser");
            ModelState.Remove("compagnieQuestions");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("employee");
          

            if (ModelState.IsValid)
            {
                db.Entry(compagnie).State = EntityState.Modified;
                db.SaveChanges();
                
                this.removeQuestionFromCompagnie(compagnie, questionToRemove);
                this.AddQuestions(compagnie, questionToAdd);

                this.removeUserFromCompagnie(compagnie, usersToRemove);
                this.AddUsers(compagnie, userToAdd);

                

                return RedirectToAction("index");
            }
            var _users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(_users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(compagnie);
        }

        // GET: CompagnieController/Delete/5
        public ActionResult Delete(int id)
        {
            var compagnie = this.db.Compagnie.Where(x=>x.id == id).Include(x=>x.compagnieUser).Include(x=>x.compagnieQuestions).FirstOrDefault();
            if (compagnie == null)
            {
                return NotFound("compagnie not found");
            }
            this.removeQuestionFromCompagnie(compagnie, compagnie.compagnieQuestions.Select(x=>x.question).ToList());
            this.removeUserFromCompagnie(compagnie, compagnie.compagnieUser.Select(x => x.user).ToList());

            this.db.Compagnie.Remove(compagnie);
            this.db.SaveChanges();
            return RedirectToAction("index");
        }

         

        #region Questions

        private List<Question> questionToAdd(List<Question> currentQuestions, List<Question> fetchedQuestions)
        {
            var intersection = currentQuestions.Intersect(fetchedQuestions);
            return fetchedQuestions.Except(currentQuestions).ToList();
        }
        private List<Question> questionToRemove(List<Question> currentQuestions, List<Question> fetchedQuestions)
        {
            var intersection = currentQuestions.Intersect(fetchedQuestions);
            return currentQuestions.Except(fetchedQuestions).ToList();
        }

        private void removeQuestionFromCompagnie(Compagnie compagnie, List<Question> questions)
        {
            var listToRemove = this.db.CompagnieQuestions.Where(x => questions.Contains(x.question) && x.compagnie.id == compagnie.id);
            this.db.CompagnieQuestions.RemoveRange(listToRemove);
            this.db.SaveChanges();
        }

        private void AddQuestions(Compagnie compagnie, List<Question> questions)
        {
            List<CompagnieQuestion> compagnieQuestions = new();
            
            foreach (var question in questions)
            {
               
                CompagnieQuestion compagnieQuestion = new CompagnieQuestion();
                compagnieQuestion.compagnieId = compagnie.id;
                compagnieQuestion.questionId = question.id;
                compagnieQuestions.Add(compagnieQuestion);
            }
            this.db.CompagnieQuestions.AddRange(compagnieQuestions);
            this.db.SaveChanges();
        }
        #endregion

        #region users

        private List<User> userToAdd(List<User> currentUsers, List<User> fetchedUsers)
        {
            var intersection = currentUsers.Intersect(fetchedUsers);
            return fetchedUsers.Except(currentUsers).ToList();
        }
        private List<User> usersToRemove(List<User> currentUsers, List<User> fetchedUsers)
        {
            var intersection = currentUsers.Intersect(fetchedUsers);
            return currentUsers.Except(fetchedUsers).ToList();
        }

        private void removeUserFromCompagnie(Compagnie compagnie, List<User> users)
        {
            var listToRemove = this.db.CompagnieUser.Where(x => users.Contains(x.user) && x.compagnie.id == compagnie.id);
            this.db.CompagnieUser.RemoveRange(listToRemove);
            this.db.SaveChanges();
        }

        private void AddUsers(Compagnie compagnie, List<User> users)
        {
            List<CompagnieUser> compagnieUsers = new();
            foreach (var user in users)
            {
                CompagnieUser compagnieUser = new CompagnieUser();
                compagnieUser.idCompagnie = compagnie.id;
                compagnieUser.userId = user.Id;
                compagnieUsers.Add(compagnieUser);
            }
            this.db.CompagnieUser.AddRange(compagnieUsers);
            this.db.SaveChanges();
        }
        #endregion

    }
}
