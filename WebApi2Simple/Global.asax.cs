using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApi2Simple
{
    public class Global : System.Web.HttpApplication
    {

        private static void RegisterAPIRoutes(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // pay attention to order of routes if there was a "defult" one

            config.Routes.MapHttpRoute(
               name: "myapi_data",
               routeTemplate: "api/{controller}/{action}",
               defaults: new { },
               constraints: new { controller = "data" });


            config.Routes.MapHttpRoute(
                name: "myapi_operations",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = "operations" });


            config.Routes.MapHttpRoute(
                name: "myapi_car",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = "car" });


            // This is one of the standard route. I do not use it. 
            // To have more control, it's better to create explicit route for controller.
            // ...imho
            //
            //config.Routes.MapHttpRoute(
            //    name: "myapi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new
            //    {
            //        id = RouteParameter.Optional,
            //    }
            //);




        }


        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(RegisterAPIRoutes);
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

        protected void Application_Error(object sender, EventArgs e)
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