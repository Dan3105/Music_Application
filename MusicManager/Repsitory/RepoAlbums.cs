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
    internal class RepoAlbums : IRepoAlbums
    {
        private readonly string api_get_albums = "api/MusicService/Album";
        private readonly string api_get_album = "api/MusicService/Album";
        private readonly string api_post_album = "api/MusicService/Album";
        private readonly string api_patch_album = "api/MusicService/Album";
        private readonly string api_delete_album = "api/MusicService/Album";
        
        private async Task AddAlbum(Album album)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(album);

                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PostAsync(api_post_album, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Add album successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task UpdateAlbum(Album album)
        {
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(album);
                var stringContent = new StringContent(jsonSerialize, UnicodeEncoding.UTF8, "application/json-patch+json");
                HttpResponseMessage responseMessage = await Axios.Client.PatchAsync(api_patch_album, stringContent);
                responseMessage.EnsureSuccessStatusCode();
                MessageBox.Show("Update album successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        
        public async Task AddAlbum(object image, Album album)
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
                    string fileImageName = $"{album.Name.ToLower().Replace(" ", "")}";
                    newImageUri = await App.FirebaseService.UpdateDataImageToCloud(imgSource, fileImageName, Config.Config.FIREBASE_ALBUM_IMG_FOLDER);
                }

                album.ImageUrl = newImageUri;
                await AddAlbum(album);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteAlbum(Album album)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.DeleteAsync(api_delete_album + $"/{album.Id}");
                responseMessage.EnsureSuccessStatusCode();
                await App.FirebaseService.DeleteFileFromCloud(album.ImageUrl, Config.Config.FIREBASE_ALBUM_IMG_FOLDER);
                MessageBox.Show("Delete album successfully");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public async Task<IEnumerable<Album>> GetAlbums()
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_albums);
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Album> albums = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Album>>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return albums;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<Album>();
            }
        }

        public async Task<Album> GetAlbumWithId(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_album + $"/{id}");
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Album album = await System.Text.Json.JsonSerializer.DeserializeAsync<Album>
                    (new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse)),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                return album;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new Album();
            }
        }

        public async Task UpdateAlbum(object image, Album album)
        {
            string newImageUri = album.ImageUrl;
            string oldImageUri = album.ImageUrl;
            try
            {
                if (image is string imageSourceLink)
                {
                    newImageUri = imageSourceLink;
                }
                else if (image is ImageSource imgSource)
                {
                    string fileImageName = $"{album.Name.ToLower().Replace(" ", "")}";
                    newImageUri = await App.FirebaseService.UpdateDataImageToCloud(imgSource, fileImageName, Config.Config.FIREBASE_ALBUM_IMG_FOLDER);
                }

                album.ImageUrl = newImageUri;
                await UpdateAlbum(album);
                if (oldImageUri != newImageUri)
                {
                    await App.FirebaseService.DeleteFileFromCloud(album.ImageUrl, Config.Config.FIREBASE_ALBUM_IMG_FOLDER);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
