using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
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
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<Artist>();
            }
        }

        private async Task AddArtist(Artist artist)
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
                HttpResponseMessage responseMessage = await Axios.Client.DeleteAsync(api_delete_artist + $"/{artist.Id}");
                responseMessage.EnsureSuccessStatusCode();
                await App.FirebaseService.DeleteFileFromCloud(artist.Image, Config.Config.FIREBASE_ARTIST_IMG_FOLDER);
                MessageBox.Show("Delete artist successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task UpdateArtist(Artist artist)
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

        public async Task AddArtist(object image, Artist artist)
        {
            string newImageUri = string.Empty;
            try
            {
                if (image is string imageSourceLink)
                {
                    newImageUri = imageSourceLink;
                }
                else if (image is ImageSource imgSource)
                {
                    string fileImageName = $"{artist.Name.ToLower().Replace(" ", "")}";
                    newImageUri = await App.FirebaseService.UpdateDataImageToCloud(imgSource, fileImageName, Config.Config.FIREBASE_SONG_IMG_FOLDER);
                }

                artist.Type = "Artiste";
                artist.Image = newImageUri;
                await AddArtist(artist);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task UpdateArtist(object image, Artist artist)
        {
            string newImageUri = artist.Image;
            string oldImageUri = artist.Image;
            try
            {
                if (image is string imageSourceLink)
                {
                    newImageUri = imageSourceLink;
                }
                else if (image is ImageSource imgSource)
                {
                    string fileImageName = $"{artist.Name.ToLower().Replace(" ", "")}";
                    newImageUri = await App.FirebaseService.UpdateDataImageToCloud(imgSource, fileImageName, Config.Config.FIREBASE_SONG_IMG_FOLDER);
                }

                artist.Image = newImageUri;
                artist.Type = "Artiste";
                await UpdateArtist(artist);
                if(oldImageUri != newImageUri)
                {
                    await App.FirebaseService.DeleteFileFromCloud(artist.Image, Config.Config.FIREBASE_ARTIST_IMG_FOLDER);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
