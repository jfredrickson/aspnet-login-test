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

        public ActionResult Administrator()
        {
            ViewBag.Message = "Administrator home page.";
            return View();
        }

        public ActionResult Analyst()
        {
            ViewBag.Message = "Analyst home page.";
            return View();
        }

        public ActionResult Respondent()
        {
            ViewBag.Message = "Respondent home page.";
            return View();
        }
    }
}