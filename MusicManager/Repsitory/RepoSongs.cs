using Microsoft.VisualBasic.ApplicationServices;
using MusicManager.Client;
using MusicManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MusicManager.Repsitory
{
    class RepoSongs : IRepoSongs
    {
        private readonly string api_get_songs = "api/Song";
        private readonly string api_get_song = "api/Song";
        private readonly string api_add_song = "api/Song/add";
        private readonly string api_delete_song = "api/Song/del";
        private readonly string api_update_song = "api/Song/update";

        public RepoSongs()
        {
        }

        public async Task<IEnumerable<Song>> GetSongs()
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_songs);
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Song> songs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Song>>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return songs;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Enumerable.Empty<Song>();
            }
        }

        public async Task AddSong(Song song)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(song);
                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PostAsync(api_add_song, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Add song successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task UpdateSong(Song song)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(song);
                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PatchAsync(api_update_song, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Update song successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task DeleteSong(Song song)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.DeleteAsync(api_delete_song + $"/{song.Id}"); ;
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Delete song successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task<Song> GetSong(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync($"{api_get_song}/{id}");
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Song song = await System.Text.Json.JsonSerializer.DeserializeAsync<Song>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Song();
            }
        }
    }
}
