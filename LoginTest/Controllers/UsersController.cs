using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoginTest.DAL;
using LoginTest.Models;
using LoginTest.ViewModels;
using System.Data.Entity.Infrastructure;

namespace LoginTest.Controllers
{
    public class UsersController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Include(u => u.Roles).Where(u => u.ID == id).Single();
            PopulateRoleAssignments(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedRoles)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Include(u => u.Roles).Where(u => u.ID == id).Single();
            if (TryUpdateModel(user, new string[] { "Name" }))
            {
                try
                {
                    UpdateUserRoleAssignments(selectedRoles, user);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Details", new { id = id });
                }
                catch (RetryLimitExceededException e)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            PopulateRoleAssignments(user);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void PopulateRoleAssignments(User user)
        {
            var allRoles = db.Roles;
            var viewModel = new List<RoleAssignment>();
            foreach (var role in allRoles)
            {
                viewModel.Add(new RoleAssignment
                {
                    RoleID = role.ID,
                    Name = role.Name,
                    Assigned = user.Roles.Contains(role)
                });
            }
            ViewBag.RoleAssignments = viewModel;
        }

        private void UpdateUserRoleAssignments(string[] selectedRoles, User user)
        {
            if (selectedRoles == null)
            {
                user.Roles = new List<Role>();
                return;
            }

            foreach (var role in db.Roles)
            {
                if (selectedRoles.Contains(role.ID.ToString()))
                {
                    // Add role to user if user does not already have the role
                    if (!user.Roles.Contains(role))
                    {
                        user.Roles.Add(role);
                    }
                }
                else
                {
                    // Remove role from user if user currently has the role
                    if (user.Roles.Contains(role))
                    {
                        user.Roles.Remove(role);
                    }
                }
            }
        }
    }
}
