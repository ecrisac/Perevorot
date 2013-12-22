using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Repositories.Repositories;

namespace Perevorot.Web.ResourceLocator
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<LoginRepository>()
                                      .BasedOn(typeof (IRepository))
                                      .WithService.AllInterfaces()
                                      .LifestyleTransient());

            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy(typeof (UnitOfWorkFactory)));
        }
        
    }
}