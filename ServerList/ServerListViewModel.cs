using Caliburn.Micro;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Testio.Login;
using Testio.Services;
using Testio.Shell;

namespace Testio.ServerList
{
    public class ServerListViewModel : Screen
    {
        public Collection<ServerModel> Servers { get; set; }

        public IAccountService AccountService { get; set; }
        public IEventAggregator Aggregator { get; set; }
        public IDataService DataService { get; set; }

        private Logger logger;

        public ServerListViewModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        protected override async void OnInitialize()
        {

            var servers = Enumerable.Empty<ServerModel>();

            try
            {
                servers = await DataService.GetServersAsync(AccountService.BearerToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to get server list");
            }

#if DEBUG
            await Task.Run(() => Thread.Sleep(2000));
#endif

            Servers.Clear();
            foreach (var server in servers)
            {
                Servers.Add(server);
            }
        }

        public void Logout()
        {
            AccountService.Logout();
            Aggregator.PublishOnUIThread(new ChangeViewMessage(typeof(LoginViewModel)));
        }

    }
}
