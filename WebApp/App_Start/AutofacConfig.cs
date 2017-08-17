using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Business;
using DataAccess;
using Owin;

namespace WebApp
{
    public static class AutofacConfig
    {
        public static void ConfigureAutofacContainer(this IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<SjContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerRequest();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacMvc();
        }
    }
}