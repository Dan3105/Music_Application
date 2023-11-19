using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Repsitory
{
    public interface IRepoAuthentication
    {
       Task<AuthenticateModel> Authenticate(string username, string password);
    }
}
