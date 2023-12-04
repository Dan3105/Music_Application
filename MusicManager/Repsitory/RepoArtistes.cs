using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using MusicManager.Client;
using MusicManager.Model;
using Newtonsoft.Json;

namespace MusicManager.Repsitory
{
    class RepoArtistes : IRepoArtistes
    {
        private readonly string api_get_artists = "api/Artist";
        private readonly string api_add_artist = "api/Artist";
        private readonly string api_update_artist = "api/Artist";
        private readonly string api_delete_artist = "api/Artist";
        private readonly string api_get_artist_with_songs = "api/Artist/with-songs";
        public RepoArtistes() { 
            
        }

        public async Task<IEnumerable<Artist>> GetArtistsAsync()
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_artists);
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Artist> artists = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Artist>>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return artists;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return Enumerable.Empty<Artist>();
            }
        }

        public async Task AddArtist(Artist artist)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(artist);

               var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PostAsync(api_add_artist, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Add artist successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task DeleteArtist(Artist artist)
        {
            try
            {
                if(artist.Songs.Count > 0)
                {
                    MessageBox.Show("Cannot delete this artist since they have songs in DB", "Failed Removed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                HttpResponseMessage responseMessage = await Axios.Client.DeleteAsync(api_delete_artist + $"/{artist.Id}");
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Delete artist successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task UpdateArtist(Artist artist)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(artist);
                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PatchAsync(api_update_artist, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Update artist successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task<Artist> GetArtistWithSongsAsync(Artist artist)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_artist_with_songs + $"/{artist.Id}");
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Artist _artist = await System.Text.Json.JsonSerializer.DeserializeAsync<Artist>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return _artist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new Artist();
            }
        }
    }
}
