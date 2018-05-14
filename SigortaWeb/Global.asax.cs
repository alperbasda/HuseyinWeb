using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SigortaWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
            Application.Add("visitor", 0);
            Application["visitor"] = 0;
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }
        void Session_Start(object sender, EventArgs e)
        {
            // Kullanýcý herhangi bir sayfadan siteme ziyarete geldiðinde visitor deðiþkenime +1 ekliyorum.
            //Ayný anda 2 veya daha fazla kullanýcý visitor deðiþkenime deðer atamasý yapmasýn diye Application.Lock() ile kilitliyorum.
            Application.Lock();
            Application["visitor"] = Convert.ToInt32(Application["visitor"]) + 1;
            Application.UnLock();
        }
        void Application_End(object sender, EventArgs e)
        {
            // Uygulama sonlandýðýnda Application State deðiþkenimi siliyorum.
            Application.Remove("visitor");
        }
    }
}