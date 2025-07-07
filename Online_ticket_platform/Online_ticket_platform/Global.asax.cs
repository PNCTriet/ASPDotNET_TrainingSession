using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Online_ticket_platform
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie == null)
                return;

            try
            {
                // Decrypt the authentication ticket
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null)
                    return;

                // Create the identity and principal
                string[] roles = { authTicket.UserData }; // UserData contains the role
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                    new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms"), 
                    roles);
            }
            catch
            {
                // If there's an error, remove the cookie
                if (authCookie != null)
                {
                    authCookie.Expires = DateTime.Now.AddDays(-1);
                    Context.Response.Cookies.Add(authCookie);
                }
            }
        }
    }
}