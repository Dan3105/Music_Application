using Microsoft.AspNetCore.JsonPatch;
using MusicManager.Client;
using MusicManager.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace MusicManager.Repsitory
{
    public class RepoUser : IRepoUser
    {
        private readonly string api_get_users = "api/UserService/User";
        private readonly string api_get_user = "api/UserService/User";
        private readonly string api_patch_user = "api/UserService/User";
        private readonly string api_get_roles = "api/UserService/Role";
        public RepoUser()
        {
        }

        public async Task<User> GetUser(int id)
        {
            if (App.AuthenticateModel == null)
            {
                MessageBox.Show("Cannot find authentication");
                return null;
            }

            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync($"{api_get_user}/{id}");
                responseMessage.EnsureSuccessStatusCode();

                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                User user = await System.Text.Json.JsonSerializer.DeserializeAsync<User>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            if(App.AuthenticateModel == null)
            {
                MessageBox.Show("Cannot find authentication");
                return Enumerable.Empty<Model.Role>();
            }

            try
            {
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_roles);


                responseMessage.EnsureSuccessStatusCode();



                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Model.Role> roles = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Model.Role>>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return roles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<Model.Role>();
            }
        }

        public async Task<IEnumerable<Model.User>> GetUsers()
        {
            if(App.AuthenticateModel == null)
            {
                MessageBox.Show("Cannot find authentication");
                return Enumerable.Empty<Model.User>();
            }
            
            try
            {
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_users);


                responseMessage.EnsureSuccessStatusCode();
                


                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Model.User> users = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Model.User>>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return users;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<Model.User>();
            }
        }

        public async Task<bool> PatchUser(User user)
        {
            if (App.AuthenticateModel == null)
            {
                MessageBox.Show("Cannot find authentication");
                return false;
            }

            try
            {
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                string jsonSerialize = JsonConvert.SerializeObject(user);
                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PatchAsync(api_patch_user, stringContent);
                responseMessage.EnsureSuccessStatusCode();


                return true;    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
