using LoginTest.DAL;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace LoginTest.Modules
{
    public class AuthenticationOverride : IHttpModule, IRequiresSessionState
    {
        private AppContext db = new AppContext();

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(PreRequestHandlerExecute);
        }

        public void PreRequestHandlerExecute(Object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            var context = app.Context;
            var usernameOverride = context.Request.QueryString["loginUsername"];
            var testHostnames = WebConfigurationManager.AppSettings["testHostnames"].Split(',');
            var testEnvironment = testHostnames.Contains(context.Request.Url.Host);
            var user = db.Users.SingleOrDefault(u => u.Name.Equals(usernameOverride));
            // If we have a query string parameter, set the session user override
            if (usernameOverride != null && testEnvironment && user != null)
            {
                GenericIdentity identity = new GenericIdentity(usernameOverride);
                GenericPrincipal principal = new GenericPrincipal(identity, user.Roles.Select(r => r.Name).ToArray());
                context.Session["principalOverride"] = principal;
            }
            // If we have a session user override, set the context user principal
            if (context.Session != null && context.Session["principalOverride"] != null && testEnvironment)
            {
                context.User = (IPrincipal)context.Session["principalOverride"];
            }
        }
    }
}
