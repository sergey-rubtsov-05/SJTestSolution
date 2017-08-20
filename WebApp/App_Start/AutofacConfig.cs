using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Business;
using DataAccess;
using Owin;
using WebApp.Engine.Maps;
using WebApp.Engine.Security;
using Module = Autofac.Module;

namespace WebApp
{
    public static class AutofacConfig
    {
        public static void ConfigureAutofacContainer(this IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<DataAccessModule>();

            builder.RegisterModule<BusinessModule>();

            builder.RegisterModule<SecurityModule>();

            builder.RegisterModule<AutoMapperModule>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacLifetimeScopeInjector(container);
            app.UseAutofacMvc();
        }
    }

    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerRequest();
        }
    }

    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticateMiddleware>().AsSelf();
            builder.RegisterType<UserContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<JwtSecurityTokenHandler>().AsSelf();
        }
    }

    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SjContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
        }
    }

    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mapperConfiguration = AutoMapperConfigurator.Configure();
            var mapper = mapperConfiguration.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
        }
    }
}