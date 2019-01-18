namespace Testio.Services
{
    public interface IAccountService
    {
        string BearerToken { get; set; }
        void Logout();
    }

    public class AccountService : IAccountService
    {
        public string BearerToken { get; set; }

        public void Logout()
        {
            BearerToken = null;
        }

    }
}
