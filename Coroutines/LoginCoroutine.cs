using Caliburn.Micro;
using NLog;
using System;
using System.Threading.Tasks;
using Testio.Services;

namespace Testio.Coroutines
{
    public class LoginCoroutine : IResult
    {
        public bool IsSuccessful { get; private set; }
        public Exception Exception { get; private set; }
        public string BearerToken { get; private set; }
        public event EventHandler<ResultCompletionEventArgs> Completed;

        private readonly string username;
        private readonly string password;
        private readonly IDataService dataService;
        private readonly Logger logger;

        public LoginCoroutine(string username, string password, IDataService dataService)
        {
            this.username = username;
            this.password = password;
            this.dataService = dataService;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public async void Execute(CoroutineExecutionContext context)
        {
            await Authenticate(username, password);
        }

        private async Task Authenticate(string username, string password)
        {
            try
            {
                BearerToken = await dataService.AuthenticateAsync(username, password);

                IsSuccessful = true;
                Completed.Invoke(this, new ResultCompletionEventArgs());
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                Exception = ex;
                logger.Error(ex, $"Unable to authenticate user '{username}':");

                Completed.Invoke(this, new ResultCompletionEventArgs());
            }
        }
    }
}
