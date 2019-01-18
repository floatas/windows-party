using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Testio.ServerList;

namespace Testio.Services
{
    public interface IDataService
    {
        Task<IEnumerable<ServerModel>> GetServersAsync(string token);
        Task<string> AuthenticateAsync(string username, string password);
    }

    public class DataService : IDataService
    {
        private class LoginResponse
        {
            public string Token { get; set; }
        }

        private class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private const string AUTH_API_URI = @"http://playground.tesonet.lt/v1/tokens";
        private const string SERVERS_API_URI = @"http://playground.tesonet.lt/v1/servers";

        public async Task<IEnumerable<ServerModel>> GetServersAsync(string token)
        {
            var servers = Enumerable.Empty<ServerModel>();

            HttpWebRequest request = WebRequest.CreateHttp(SERVERS_API_URI);
            request.ContentType = "application/json";
            request.Method = "Get";

            request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token}");

            var response = await request.GetResponseAsync();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseText = reader.ReadToEnd();

                servers = JsonConvert.DeserializeObject<IEnumerable<ServerModel>>(responseText);
            }

#if DEBUG
            await Task.Run(() => Thread.Sleep(2000));
#endif
            return servers;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            string bearerToken = null;

            HttpWebRequest request = WebRequest.CreateHttp(AUTH_API_URI);
            request.ContentType = "application/json";
            request.Method = "Post";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonConvert.SerializeObject(new { Username = username, Password = password }, Formatting.None,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            }

            var response = await request.GetResponseAsync();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseText = reader.ReadToEnd();

                var result = JsonConvert.DeserializeObject<LoginResponse>(responseText);
                bearerToken = result.Token;
            }

#if DEBUG
            await Task.Run(() => Thread.Sleep(2000));
#endif
            return bearerToken;
        }

    }
}
