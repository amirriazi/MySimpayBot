using Shared.WebService;
using System.Linq;
using System.Web.Http;

namespace MySimpayBot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            // Web API configuration and services
            //config.MessageHandlers.Add(New MessageHandler())
            // Web API routes
            config.MapHttpAttributeRoutes();

            // Controller Only
            // To handle routes like "v1/api/Participants"
            config.Routes.MapHttpRoute(name: "ControllerOnly", routeTemplate: "v1/api/{controller}");

            // Controller with ID
            // To handle routes like "v1/api/Participants/9121014856"
            // Only integers 
            config.Routes.MapHttpRoute(name: "ControllerAndId", routeTemplate: "v1/api/{controller}/{id}", defaults: null, constraints: new { id = "^\\d+$" });

            // Controllers with Actions
            // To handle routes like "v1/api/Participants/set"
            config.Routes.MapHttpRoute(name: "ControllerAndAction", routeTemplate: "v1/api/{controller}/{action}");

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());


        }
    }
}
