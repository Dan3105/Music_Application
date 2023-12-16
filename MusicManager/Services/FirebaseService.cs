using Firebase.Storage;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicManager.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseStorage _storage;
        public FirebaseService()
        {
            _storage = new FirebaseStorage(Config.Config.FIREBASE_STORAGE);
        }
        public async Task<string> UpdateDataImageToCloud(ImageSource imageSource, string fileName, string folderFileConfig)
        {
            try
            {
                if (imageSource != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bitmapSource = imageSource as BitmapSource;

                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(ms);

                        ms.Seek(0, SeekOrigin.Begin);

                        var urlUpload = await _storage
                        .Child($"{folderFileConfig}/{fileName}{DateTime.Now.ToString("yyyyMMddHHmmss")}.png")
                        .PutAsync(ms);

                        Console.WriteLine(urlUpload);
                        return urlUpload;
                    }
                }
                MessageBox.Show("Image is null");
                return string.Empty;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return string.Empty;

            }
        }
        
        public async Task<string> UpdateDataSongToCloud(string filePath, string fileName, string folderFileConfig)
        {
            string urlLoad = string.Empty;
            try
            {
                var validateUri = new Uri(filePath);
                if (validateUri.IsFile)
                {
                    using (var stream = File.Open(validateUri.LocalPath, FileMode.Open))
                    {
                        urlLoad = await _storage
                            .Child($"{folderFileConfig}/{fileName}{DateTime.Now.ToString("yyyyMMddHHmmss")}.mp3")
                            .PutAsync(stream);

                        return urlLoad;
                    }
                }
                else
                {
                    return filePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return urlLoad;
            }
        }

        public async Task DeleteFileFromCloud(string url, string folderFileConfig)
        {
            try
            {
                Uri newUri = new Uri(url);
                string path = newUri.LocalPath;
                path = Uri.UnescapeDataString(path);

                int indexOfLastSlash = path.LastIndexOf('/');
                string desiredPath = path.Substring(indexOfLastSlash + 1);
               
                await _storage.Child(folderFileConfig).Child(desiredPath).DeleteAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
