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
            if (usernameOverride != null && testEnvironment && db.Users.Where(u => u.Name.Equals(usernameOverride)).Any())
            {
                GenericIdentity identity = new GenericIdentity(usernameOverride);
                GenericPrincipal principal = new GenericPrincipal(identity, new string[] { });
                context.Session["userOverride"] = principal;
            }
            if (context.Session != null && context.Session["userOverride"] != null && testEnvironment)
            {
                context.User = (IPrincipal)context.Session["userOverride"];
            }
        }
    }
}
