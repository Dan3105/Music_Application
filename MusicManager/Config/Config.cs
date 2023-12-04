using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Config
{
    public class Config
    {
        public enum ImageSourceType
        {
            File,
            Url,
            None
        }

        public static string REQUEST_API = "http://localhost:5070/";
        public static string FIREBASE_STORAGE = "musicproject-2737f.appspot.com";
        public static string FIREBASE_SONG_MP3_FOLDER = "mp3-song";
        public static string FIREBASE_ARTIST_IMG_FOLDER = "img-artist";
        public static string FIREBASE_SONG_IMG_FOLDER = "img-song";
    }
}
