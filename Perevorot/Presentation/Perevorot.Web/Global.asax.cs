using System.Data.Entity;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Web.ResourceLocator;

namespace Perevorot.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            IoC.RegisterAll(); 
            base.Init();
        }
        
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            AddJsonFormatterAndSetDefault();
            IoC.RegisterAll();           
            //Database.SetInitializer(new DatabaseInitializer());
                      
        }


        protected void Application_End()
        {
            IoC.Dispose();
        }

        private static void AddJsonFormatterAndSetDefault()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            var jsonFormatter = new JsonNetFormatter(serializerSettings);
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            GlobalConfiguration.Configuration.Formatters.Insert(0, jsonFormatter);
        }
    }
 
}