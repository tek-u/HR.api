using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HR.api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            string headers = "Accept, Origin, Content-Type, X-Auth-Token, cache-control, x-requested-with",
                   methods = "GET, POST ,PATCH, PUT, DELETE, OPTIONS",
            origin = ConfigurationManager.AppSettings["CORS_local"];
            //origin = ConfigurationManager.AppSettings["CORS_prod"];

            var cors = new EnableCorsAttribute(origin, headers, methods);
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
