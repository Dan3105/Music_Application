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
using MusicManager.Client;
using MusicManager.Model;
namespace MusicManager.Repsitory
{
    class RepoArtistes : IRepoArtistes
    {
        private readonly string api_get_artists = "api/Artist";
        public RepoArtistes() { 
            
        }

        public async Task<IEnumerable<Artist>> GetArtistsAsync()
        {
            try
            {
                HttpResponseMessage responseMessage = await Axios.Client.GetAsync(api_get_artists);
                responseMessage.EnsureSuccessStatusCode();
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                List<Artist> artists = await JsonSerializer.DeserializeAsync<List<Artist>>
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
    }
}
