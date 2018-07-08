using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using X.IDao;
using X.IService;
using X.IService.Authentication;
using X.Common;
using X.Dao;

namespace X.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var db = DbSession.GetCurrentDbContext();
            db.Database.CreateIfNotExists();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterAutofac();
            X.Service.Initializer.AutoMapperInit();

        }
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            var baseType = typeof(IDependency);
            var assemblys = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<System.Reflection.Assembly>().ToList();
            builder.RegisterControllers(assemblys.ToArray());
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                   .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                   .AsImplementedInterfaces().InstancePerLifetimeScope().InstancePerRequest();
            builder.Register(ctx => RouteTable.Routes).SingleInstance();
            builder.Register(ctx => ModelBinders.Binders).SingleInstance();
            builder.Register(ctx => ViewEngines.Engines).SingleInstance();
            IContainer container = builder.Build();
            Common.DIContainer.Initialize(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
