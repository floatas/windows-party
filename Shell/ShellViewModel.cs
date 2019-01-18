using Caliburn.Micro;
using Autofac;
using Testio.Login;
using System;
using Testio.Services;

namespace Testio.Shell
{
    public class ShellViewModel : Conductor<object>, IHandle<ChangeViewMessage>
    {
        public IEventAggregator Aggregator { get; set; }

        public ShellViewModel()
        {
            Container.IoC.InjectUnsetProperties(this);

            Aggregator.Subscribe(this);

            NavigateToView(typeof(LoginViewModel));
        }

        public void Handle(ChangeViewMessage message)
        {
            NavigateToView(message.ViewModelType);
        }

        private void NavigateToView(Type viewType)
        {
            var instance = Activator.CreateInstance(viewType);
            ActivateItem(Container.IoC.InjectUnsetProperties(instance));
        }
    }
}
