using AutoMapper;
using AutoMapper.Configuration;
using Strata_WebAPI_Exercise.Mappings;
using System.Web.Http;

namespace Strata_WebAPI_Exercise
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //Avoid self refernce loop errors in JSON serialisation
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            var cfg = new MapperConfigurationExpression();
            
            cfg.AddProfile<MapEntityToDto>();
            Mapper.Initialize(cfg);
        }
    }
}
