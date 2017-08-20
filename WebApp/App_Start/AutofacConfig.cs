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

namespace WebApp
{
    public static class AutofacConfig
    {
        public static void ConfigureAutofacContainer(this IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<SjContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerRequest();
            builder.RegisterType<AuthenticateMiddleware>().AsSelf();
            builder.RegisterType<UserContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<JwtSecurityTokenHandler>().AsSelf();

            var mapperConfiguration = AutoMapperConfigurator.Configure();
            var mapper = mapperConfiguration.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacLifetimeScopeInjector(container);
            app.UseAutofacMvc();
        }
    }
}