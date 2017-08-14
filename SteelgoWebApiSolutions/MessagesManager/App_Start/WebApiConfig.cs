using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MessagesManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "MessagesManagerApi",
                routeTemplate: "messagesmanager/api/{controller}/{username}/{token}/{parameter}",
                defaults: new
                {
                    parameter = RouteParameter.Optional,
                    username = RouteParameter.Optional,
                    token = RouteParameter.Optional
                }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
