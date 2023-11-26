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
        private IRepoAuthentication repoAuthentication;
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

        private IRepoArtistes repoArtistes;
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

        private IRepoSongs repoSongs;
        public IRepoSongs RepoSongs
        {
            get
            {
                if(repoSongs == null)
                    repoSongs = new RepoSongs();
                return repoSongs;
            }
        }

        private IRepoUser repoUsers;
        public IRepoUser RepoUsers
        {
            get
            {
                if (repoUsers == null)
                {
                    repoUsers = new RepoUser();
                }
                return repoUsers;
            }
        }
    }
}
