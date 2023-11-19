using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expired { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expired { get; set; }
    }

    public class UserRequest
    {
        public string UserEmail { get; set; }
        public List<int> Roles { get; set; }
        public List<int> Favorites { get; set; }
    }

    public class AuthenticateModel
    {
        public AccessToken AccessToken { get; set; }
        public bool IsSuccess { get; set; }
        public string? Reason { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public UserRequest UserRequest { get; set; }
    }

}
