using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Repsitory
{
    public class RepositoryManager
    {
        public static RepositoryManager? Instance
        {
            get;
            private set;
        }

        public RepositoryManager() {
            Instance = this;
        }

        //List of Repohandler
        RepoAuthentication repoAuthentication;
        public IRepoAuthentication RepoAuthentication
        {
            get
            {
                if(repoAuthentication == null)
                {
                    repoAuthentication = new RepoAuthentication();
                }
                return repoAuthentication;
            }
        }

        RepoArtistes repoArtistes;
        public IRepoArtistes RepoArtistes
        {
            get
            {
                if (repoArtistes == null)
                {
                    repoArtistes = new RepoArtistes();
                }
                return repoArtistes;
            }
        }

        RepoSongs repoSongs;
        public IRepoSongs RepoSongs
        {
            get
            {
                if(repoSongs == null)
                    repoSongs = new RepoSongs();
                return repoSongs;
            }
        }
    }
}
