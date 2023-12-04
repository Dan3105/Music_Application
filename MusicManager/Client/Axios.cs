using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Client
{
    public class Axios
    {
        private static HttpClient _client;

        public static HttpClient Client
        {
            get
            {
                if(_client == null)
                {
                    _client = new HttpClient();
                    _client.BaseAddress = new Uri(Config.Config.REQUEST_API);
                }
                    
                return _client;
            }
        }


    }
}
