using Strata_WebAPI_Exercise.Interfaces;
using Strata_WebAPI_Exercise.Resolver;
using Strata_WebAPI_Exercise.Services;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace Strata_WebAPI_Exercise
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            // Singleton repository, to retain changes to the data during runtime
            container.RegisterType<IRepositoryService, RepositoryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICustomerService, CustomerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IShoppingCartService, ShoppingCartService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrderService, OrderService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

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
