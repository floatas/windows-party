using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows;
using Testio.Coroutines;
using Testio.ServerList;
using Testio.Services;
using Testio.Shell;

namespace Testio.Login
{
    public class LoginViewModel : PropertyChangedBase
    {
#if DEBUG
        private string userName = "tesonet";
        private string password = "partyanimal";
#else
        private string userName;
        private string password;
#endif


        private bool isSigningIn;

        public IEventAggregator Aggregator { get; set; }
        public IAccountService AccountService { get; set; }
        public IDataService DataService { get; set; }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                NotifyOfPropertyChange(nameof(UserName));
                NotifyOfPropertyChange(nameof(CanLogin));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyOfPropertyChange(nameof(Password));
                NotifyOfPropertyChange(nameof(CanLogin));
            }
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(userName)
                        && !string.IsNullOrEmpty(password)
                        && !isSigningIn;
            }
        }

        private bool IsSigningIn
        {
            get
            {
                return isSigningIn;
            }
            set
            {
                isSigningIn = value;
                NotifyOfPropertyChange(nameof(CanLogin));
            }
        }

        public IEnumerable<IResult> Login()
        {
            IsSigningIn = true;

            var loginCoroutine = new LoginCoroutine(UserName, Password, DataService);
            yield return loginCoroutine;

            if (loginCoroutine.IsSuccessful)
            {
                AccountService.BearerToken = loginCoroutine.BearerToken;
                Aggregator.PublishOnUIThread(new ChangeViewMessage(typeof(ServerListViewModel)));
            }
            else
            {
                MessageBox.Show(loginCoroutine.Exception.Message, "Unable to login", MessageBoxButton.OK);
            }

            IsSigningIn = false;
        }

    }
}
