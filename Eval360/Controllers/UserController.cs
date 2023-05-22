using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    public class UserController : Controller
    {

        private UserManager<User> userManager;
        private ApplicationDbContext db;
        //todo finish profile editing
        public UserController(UserManager<User> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(this.db.User.Include(x => x.Poste).ToArray());
        }

        [HttpGet]
        public async Task<IActionResult> create()
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");

            return View(new User());

        }

        // POST: PosteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                
                var result = await this.userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    string role = user.UserType.ToString();
                    await this.userManager.AddToRoleAsync(user, role);
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



        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
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
                await this.userManager.RemovePasswordAsync(user);
                await this.userManager.AddPasswordAsync(user, user.PasswordHash);
            }
            var userToUpdate = this.userManager.FindByIdAsync(user.Id);
            if (userToUpdate == null)
            {
                return View(user);
            }


            // Update the user in the database.
            var result = await this.userManager.UpdateAsync(this.updateUserFields(userToUpdate.Result, user));
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

        private User updateUserFields(User userToUpdate, User currentUser)
        {
            if (!userToUpdate.Email.Equals(currentUser.Email))
            {
                userToUpdate.Email = currentUser.Email;
            }
            if (!userToUpdate.cin.Equals(currentUser.cin))
            {
                userToUpdate.cin = currentUser.cin;
            }
            if (!userToUpdate.Nom.Equals(currentUser.Nom))
            {
                userToUpdate.Nom = currentUser.Nom;
            }
            if (!userToUpdate.preNom.Equals(currentUser.preNom))
            {
                userToUpdate.preNom = currentUser.preNom;
            }
            if (!userToUpdate.sexe.Equals(currentUser.sexe))
            {
                userToUpdate.sexe = currentUser.sexe;
            }
            if (!userToUpdate.idPoste.Equals(currentUser.idPoste))
            {
                userToUpdate.idPoste = currentUser.idPoste;
            }
            if (!userToUpdate.idSuperior.Equals(currentUser.idSuperior))
            {
                userToUpdate.idSuperior = currentUser.idSuperior;
            }

            return userToUpdate;
        }

    }



}

