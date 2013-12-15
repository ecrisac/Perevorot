using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Perevorot.Domain.IServices;
using Perevorot.Domain.Services;

namespace Perevorot.Web.ResourceLocator
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            //container.Register(AllTypes.FromAssemblyNamed("Perevorot.Domain.IServices")
            //               .Where(type => type.Name.EndsWith("Service")).LifestylePerWebRequest());

            //container.Register(AllTypes.FromAssemblyNamed("Perevorot.Domain.Services")
            //                         .Where(type => type.Name.EndsWith("Service"))
            //                          .LifestylePerWebRequest());



            container.Register(Classes.FromAssemblyContaining<LoginService>()
                .BasedOn(typeof(IService))
                .WithService.AllInterfaces()
                .LifestyleTransient()); 
        }
    }
}