using Autofac;
using Caliburn.Micro;

namespace Testio.Services
{
    public static class Container
    {
        public static IContainer IoC;

        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized)
            {
                return;
            }

            var builder = new ContainerBuilder();

            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<DataService>().As<IDataService>().SingleInstance();

            IoC = builder.Build();
            initialized = true;
        }
    }
}
