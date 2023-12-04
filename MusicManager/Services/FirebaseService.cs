using Firebase.Storage;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicManager.Services
{
    public class FirebaseService : IFirebaseService
    {
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

                        var urlUpload = await new FirebaseStorage(Config.Config.FIREBASE_STORAGE)
                        .Child($"{Config.Config.FIREBASE_ARTIST_IMG_FOLDER}/{fileName}{DateTime.Now.ToString("yyyyMMddHHmmss")}.png")
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
                        var firebaseStorage = new FirebaseStorage(Config.Config.FIREBASE_STORAGE);
                        urlLoad = await firebaseStorage
                            .Child($"{Config.Config.FIREBASE_SONG_MP3_FOLDER}/{fileName}{DateTime.Now.ToString("yyyyMMddHHmmss")}.mp3")
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
    }
}
