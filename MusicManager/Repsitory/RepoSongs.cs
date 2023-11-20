using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicManager.Repsitory
{
    class RepoSongs : IRepoSongs
    {
        private readonly string api_get_songs = "api/Song";
        static HttpClient client = new HttpClient();

        public RepoSongs()
        {
            client.BaseAddress = new Uri(Config.Config.REQUEST_API);
        }

        public async Task<IEnumerable<Song>> GetSongs()
        {
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(api_get_songs);
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Song> songs = await JsonSerializer.DeserializeAsync<List<Song>>
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
    }
}
