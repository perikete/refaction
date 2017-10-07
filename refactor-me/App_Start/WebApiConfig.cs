using System.Web.Http;
using System.Web.Http.Dispatcher;
using refactor_me.Infrastructure;

namespace refactor_me
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new PoorManControllerActivator());
        }
    }
}
