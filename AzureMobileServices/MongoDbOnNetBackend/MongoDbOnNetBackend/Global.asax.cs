using System;

namespace MongoDbOnNetBackend
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register();
        }
    }
}