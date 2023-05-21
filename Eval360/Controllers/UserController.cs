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
        public IActionResult create()
        {
            ViewBag.PostesList = new SelectList(this.db.Poste.ToArray(), "Id", "libelle");
            ViewBag.UserList = new SelectList(this.db.User.Select(x => new {Id = x.Id, libelle = x.Nom+" "+x.preNom}).ToArray(), "Id", "libelle");

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
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);

        }

        // GET: PosteController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Get the user from the database.
            var user = await this.userManager.FindByIdAsync(id.ToString());

            // If the user is not found, return a 404 error.
            if (user == null)
            {
                return NotFound();
            }

            // Return the view.
            return View(user);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            // Validate the user input.
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Update the user in the database.
            var result = await this.userManager.UpdateAsync(user);
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


    }

}

