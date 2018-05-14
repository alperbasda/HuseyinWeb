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
            // Kullan�c� herhangi bir sayfadan siteme ziyarete geldi�inde visitor de�i�kenime +1 ekliyorum.
            //Ayn� anda 2 veya daha fazla kullan�c� visitor de�i�kenime de�er atamas� yapmas�n diye Application.Lock() ile kilitliyorum.
            Application.Lock();
            Application["visitor"] = Convert.ToInt32(Application["visitor"]) + 1;
            Application.UnLock();
        }
        void Application_End(object sender, EventArgs e)
        {
            // Uygulama sonland���nda Application State de�i�kenimi siliyorum.
            Application.Remove("visitor");
        }
    }
}