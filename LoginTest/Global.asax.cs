using LoginTest.DAL;
using LoginTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LoginTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create initial admin user if none exists
            using (var db = new AppContext())
            {
                string adminUsername;
                var adminRole = db.Roles.SingleOrDefault(r => r.Name.Equals("Administrator"));
                if (!adminRole.Users.Any())
                {
                    using (var fs = System.IO.File.OpenText(HttpContext.Current.Server.MapPath("~/admin.txt")))
                    {
                        adminUsername = fs.ReadLine().Trim();
                    }
                    var adminUser = db.Users.SingleOrDefault(u => u.Name.Equals(adminUsername));
                    if (adminUser == null)
                    {
                        adminUser = new User { Name = adminUsername };
                        adminUser.Roles.Add(adminRole);
                        db.Users.Add(adminUser);
                    }
                    else
                    {
                        adminUser.Roles.Add(adminRole);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
