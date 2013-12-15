using System.Web.Mvc;
using Castle.Windsor;

namespace Perevorot.Web.ResourceLocator
{
    public static class IoC
    {
        private static IWindsorContainer _container;

        public static void RegisterAll()
        {
            _container = new WindsorContainer();
            _container.Install(new ControllersInstaller());
            _container.Install(new ServicesInstaller());
            _container.Install(new RepositoriesInstaller());

            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static void Dispose()
        {
            _container.Dispose();
        }
    }
}