using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{
    public class TokenInfo
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("expired")]
        public DateTime Expired { get; set; }
    }

    public class UserRequest
    {
        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("favorites")]
        public List<int> Favorites { get; set; }
    }

    public class AuthenticateModel
    {
        [JsonProperty("accessToken")]
        public TokenInfo AccessToken { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("refreshToken")]
        public TokenInfo RefreshToken { get; set; }

        [JsonProperty("userRequest")]
        public UserRequest UserRequest { get; set; }
    }

}
