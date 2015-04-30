using System;
using SitefinityWebApp.UnlockPage;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Telerik.Sitefinity.Abstractions.Bootstrapper.Initialized += new EventHandler<Telerik.Sitefinity.Data.ExecutedEventArgs>(Bootstrapper_Initialized);
        }

        protected void Bootstrapper_Initialized(object sender, Telerik.Sitefinity.Data.ExecutedEventArgs args)
        {           
            if (args.CommandName == "Bootstrapped")
            {               
                UnlockPageHelperExtensions.Install();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}