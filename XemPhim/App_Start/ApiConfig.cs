using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace XemPhim
{
    public static class ApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigRouter(config);
            ConfigHeaders(config);
            ConfigFormatter(config);
        }

        public static void ConfigRouter(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "CRUD",
            //    routeTemplate: "{model}/{action}",
            //    defaults: new { action = RouteParameter.Optional }
            //);

            config.Routes.Add(
                name: CRUDRoute.Name,
                route: new CRUDRoute()
            );
        }

        public static void ConfigHeaders(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }

        public static void ConfigFormatter(HttpConfiguration config)
        {
            JsonMediaTypeFormatter jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }

    public class CRUDRoute : HttpRoute
    {
        public static string Name = "CRUD";
    }
}
