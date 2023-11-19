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
    }
}
