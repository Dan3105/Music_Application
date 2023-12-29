using MusicManager.Client;
using MusicManager.Model;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicManager.Repsitory
{
    public class RepoAuthentication : IRepoAuthentication
    {
        private readonly string api_login = "api/UserService/Auth/login";
        public RepoAuthentication()
        {
        }

        public async Task<AuthenticateModel> Authenticate(string _username, string _password)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.PostAsJsonAsync(api_login,
                new
                {
                    email = _username,
                    password = _password
                });

                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                AuthenticateModel responseAuth = await JsonSerializer.DeserializeAsync<AuthenticateModel>(
                    new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return responseAuth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
