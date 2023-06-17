using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Eval360.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private UserManager<User> userManager;
        private ApplicationDbContext db;

        public UserController(UserManager<User> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        [CustomAuthorization(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(this.db.User.Include(x => x.Poste).ToArray());
        }

      
        [CustomAuthorization(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> create()
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");

            return View(new User());

        }

        
        // POST: PosteController/Create
        [CustomAuthorization(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            ModelState.Remove("compagnies");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("compagnieUser");
            if (ModelState.IsValid)
            {

                var result = await this.userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    string role = user.UserType.ToString();
                    var res = await this.userManager.AddToRoleAsync(user, role);
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");

            return View(user);

        }

        
        [CustomAuthorization(Roles = "Admin")]
        // GET: PosteController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            // Get the user from the database.
            var user = await this.userManager.FindByIdAsync(id);

            // If the user is not found, return a 404 error.
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");

            // Return the view.
            return View(user);
        }


        
        [CustomAuthorization(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            var userToUpdate = await this.userManager.FindByIdAsync(user.Id);
            ModelState.Remove("compagnies");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("compagnieUser");

            if (user.PasswordHash == null)
            {
                ModelState.Remove("PasswordHash");
            }
           
            // Validate the user input.
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                string password = user.PasswordHash;
                await this.userManager.RemovePasswordAsync(userToUpdate);
                await this.userManager.AddPasswordAsync(userToUpdate, password);
            }
            
            if (userToUpdate == null)
            {
                return View(user);
            }


            // Update the user in the database.
            var result = await this.userManager.UpdateAsync(this.updateUserFields(userToUpdate, user));
            if (result.Succeeded)
            {
                // Redirect to the home page.
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }

       
        [Authorize]
        public async Task<IActionResult> profile(string username)
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            // Get the user from the database.
            var user = await this.userManager.FindByNameAsync(username);

            // If the user is not found, return a 404 error.
            if (user == null)
            {
                return NotFound();
            }


            // Return the view.
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> profile(User user)
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            bool flag = false;
            if (user.PasswordHash == null)
            {
                ModelState.Remove("PasswordHash");
            }
            ModelState.Remove("username");
            ModelState.Remove("sexe");
            ModelState.Remove("cin");
            ModelState.Remove("dateEmbauche");
            ModelState.Remove("idPoste");
            ModelState.Remove("Poste");
            ModelState.Remove("UserType");
            ModelState.Remove("idSuperior");
            ModelState.Remove("superior");
            ModelState.Remove("Email");
            ModelState.Remove("compagnies");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("compagnieUser");
            // Validate the user input.
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var userToUpdate = this.userManager.FindByNameAsync(user.UserName).Result;
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                string password = user.PasswordHash;
                await this.userManager.RemovePasswordAsync(userToUpdate);
                flag = (await this.userManager.AddPasswordAsync(userToUpdate, password)).Succeeded;
            }

            if (userToUpdate == null)
            {
                return View(user);
            }
            if (!string.IsNullOrEmpty(user.Nom) && !user.Nom.Equals(userToUpdate.Nom))
            {
                userToUpdate.Nom = user.Nom;
            }
            if (!string.IsNullOrEmpty(user.preNom) && !user.preNom.Equals(userToUpdate.preNom))
            {
                userToUpdate.preNom = user.preNom;
            }


            // Update the user in the database.

            var result = await this.userManager.UpdateAsync(userToUpdate);
            if (result.Succeeded || flag)
            {
                // Redirect to the home page.
                return RedirectToAction("Index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }

       
        
        [CustomAuthorization(Roles = "Admin")]
        public async Task<IActionResult> disable(string id)
        {
            User user = this.userManager.FindByIdAsync(id).Result;
            bool lockedOut = this.userManager.IsLockedOutAsync(user).Result;
            if (lockedOut)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddDays(5000);
            }
            await this.userManager.UpdateAsync(user);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> stat(string id)
        {
            ViewBag.axeList = this.db.AxeEval.ToArray();
            ViewBag.compagnieCount = this.db.Compagnie.Where(c=>c.userId == id).Count();
            ViewBag.responseCount = this.db.CompagnieResponse.Where(c=>c.CompagnieQuestion.compagnie.userId == id).GroupBy(x => new { x.userId, x.CompagnieQuestion.compagnieId }).Count();
            ViewBag.latestCompagnies = this.db.Compagnie.Where(c=>c.userId == id).Include(x => x.employee).OrderByDescending(x => x.dateDebut).Take(6);
            ViewBag.responseByAxe = this.db.AxeEval.Select(ae => new
            {
                AxeEvalName = ae.name,
                AverageResponse = ae.questions
                                    .SelectMany(q => q.compagnieQuestions).Where(qc=>qc.compagnie.userId == id)
                                    .SelectMany(cq => cq.reponses)
                                    .Average(r => (double?)r.note)
            }).OrderBy(a => a.AxeEvalName).Select(x => x.AverageResponse);

            ViewBag.reponseByEvaluateur = JsonConvert.SerializeObject(getResponseByQualite(id));
            ViewBag.compagnieCountByMonth = JsonConvert.SerializeObject(this.getCompagnieByMonthChart(id));

            return View();

        }

        private dynamic getCompagnieByMonthChart(string id)
        {
           
                var companiesPerMonth = new List<int>();
                for (int month = 1; month <= 12; month++)
                {
                    var companiesCreated = this.db.Compagnie
                        .Where(c => c.dateDebut.Month == month && c.userId == id);
                    companiesPerMonth.Add(companiesCreated.Count());
                }
                 
            


            return companiesPerMonth;
        }


        private dynamic getResponseByQualite(string id)
        {
            Dictionary<string, double> data = new Dictionary<string, double>() { { "Autoévaluation", 0.0 }, { "Collaborateur", 0.0 }, { "Collègue", 0.0 }, { "Hiérarchie", 0.0 } };

            foreach (string key in data.Keys)
            {
                if (this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key) && c.userId == id).Count() != 0
                    && this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key)).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Count() != 0)
                {
                    data[key] = this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key) && c.userId == id).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Average(r => r.note);
                }
            }
            return data.Values;
        }

        private User updateUserFields(User userToUpdate, User currentUser)
        {
            if (!string.Equals(userToUpdate.Email,currentUser.Email))
            {
                userToUpdate.Email = currentUser.Email;
            }
            if (!string.Equals(userToUpdate.cin,currentUser.cin))
            {
                userToUpdate.cin = currentUser.cin;
            }
            if (!string.Equals(userToUpdate.Nom,currentUser.Nom))
            {
                userToUpdate.Nom = currentUser.Nom;
            }
            if (!string.Equals(userToUpdate.preNom,currentUser.preNom))
            {
                userToUpdate.preNom = currentUser.preNom;
            }
            if (!string.Equals(userToUpdate.sexe, currentUser.sexe))
            {
                userToUpdate.sexe = currentUser.sexe;
            }
            if (!string.Equals(userToUpdate.idPoste,currentUser.idPoste))
            {
                userToUpdate.idPoste = currentUser.idPoste;
            }
            if (!string.Equals(userToUpdate.idSuperior,currentUser.idSuperior))
            {
                userToUpdate.idSuperior = currentUser.idSuperior;
            }

            return userToUpdate;
        }

    }



}

