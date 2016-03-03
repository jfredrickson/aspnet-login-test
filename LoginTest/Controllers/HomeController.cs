using LoginTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginTest.Controllers
{
    public class HomeController : Controller
    {
        private AppContext db = new AppContext();

        public ActionResult Index()
        {
            ViewBag.Roles = db.Roles.ToList();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Administrator()
        {
            ViewBag.Message = "Administrator home page.";
            return View();
        }

        [Authorize(Roles = "Analyst")]
        public ActionResult Analyst()
        {
            ViewBag.Message = "Analyst home page.";
            return View();
        }

        [Authorize(Roles = "Respondent")]
        public ActionResult Respondent()
        {
            ViewBag.Message = "Respondent home page.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}