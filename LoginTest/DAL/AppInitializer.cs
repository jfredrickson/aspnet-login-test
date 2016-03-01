using LoginTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginTest.DAL
{
    public class AppInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            // Create application roles
            var roles = new List<Role>
            {
                new Role { Name = "Administrator" },
                new Role { Name = "Analyst" },
                new Role { Name = "Respondent" }
            };
            roles.ForEach(r => context.Roles.Add(r));
            context.SaveChanges();
        }
    }
}