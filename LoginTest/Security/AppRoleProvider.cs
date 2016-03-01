using LoginTest.DAL;
using LoginTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LoginTest.Security
{
    public class AppRoleProvider : RoleProvider
    {
        private AppContext db = new AppContext();
        public override string ApplicationName { get; set; }

        public override string[] GetRolesForUser(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.Name.Equals(username));
            if (user == null)
            {
                throw new ArgumentException("Invalid user: " + username);
            }

            var result = from userRole in user.Roles
                         select userRole.Name;
            if (result != null)
            {
                return result.ToArray<string>();
            }
            else
            {
                return new string[] {};
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username).Contains(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
            /*
            foreach (var roleName in roleNames)
            {
                var role = db.Roles.FirstOrDefault(r => r.Name.Equals(roleName));
                foreach (var username in usernames)
                {
                    var user = db.Users.FirstOrDefault(u => u.Name.Equals(username));
                    user.Roles.Add(role);
                }
            }
            db.SaveChanges();
            */
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}