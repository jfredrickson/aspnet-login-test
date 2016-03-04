using LoginTest.DAL;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace LoginTest.Modules
{
    /// <summary>
    /// Allows a tester to override Windows Authentication by using the
    /// 'loginUsername' URL parameter. This module ensures that the user
    /// principal in the context is always set to the given overridding user
    /// principal. The override can be cleared by resetting the session
    /// (i.e., Session.Abandon()).
    /// 
    /// Note that the overridding username given in the URL parameter must
    /// actually be a valid user in the application.
    /// 
    /// Example: http://hostname/?loginUsername=TEST\test.user1
    /// </summary>
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
