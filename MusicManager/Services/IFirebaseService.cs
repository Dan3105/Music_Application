using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicManager.Services
{
    public interface IFirebaseService
    {
        Task<string> UpdateDataSongToCloud(string filePath, string fileName, string folderFileConfig);
        Task<string> UpdateDataImageToCloud(ImageSource imageSource, string fileName, string folderFileConfig);
    }
}
