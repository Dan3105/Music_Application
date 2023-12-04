using Firebase.Storage;
using MusicManager.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static MusicManager.Config.Config;
using MessageBox = System.Windows.MessageBox;

namespace MusicManager.View.SubView
{
    /// <summary>
    /// Interaction logic for FormEditDataSong.xaml
    /// </summary>
    public partial class FormEditDataSong : Window
    {
        private ICommand ActionSubmit;
        public ObservableCollection<Artist> _artists { get; set; }
        private ObservableCollection<Artist> _currentSongArtits { get; set; }
        private Song _currentSong { get; set; }

        public FormEditDataSong()
        {
            InitializeComponent();
        }

        public FormEditDataSong(IEnumerable<Artist> artists, ICommand callback)
        {
            InitializeComponent();
            ActionSubmit = callback;
            BindingNewDataToUI(artists);
        }


        public FormEditDataSong(IEnumerable<Artist> artists, ICommand callback, Song currentSong)
        {
            InitializeComponent();
            ActionSubmit = callback;
            _artists = new ObservableCollection<Artist>(artists);
            _currentSong = currentSong;
            BindingDataToUI(currentSong);
        }

        private void BindingNewDataToUI(IEnumerable<Artist> artists)
        {
            _currentSongArtits = new ObservableCollection<Artist>();
            _artists = new ObservableCollection<Artist>(artists);
            _currentSong = new Song();
            DGActor.ItemsSource = _currentSongArtits;
            cbArtists.ItemsSource = _artists;
            InitTimerDispatch();

        }


        private void BindingDataToUI(Song currentSong)
        {
            txbSongName.Text = currentSong.Title;
            dpDateRealease.Text = currentSong.ReleaseDate.ToString();
            _currentSongArtits = new ObservableCollection<Artist>(currentSong.Artists);
            DGActor.ItemsSource = _currentSongArtits;
            cbArtists.ItemsSource = _artists;
            txbSongUrl.Text = currentSong.SongURL;
            InitImage();
            InitSong();
            RefreshSong();
            InitTimerDispatch();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ClearBeforeClose();
            this.Close();
        }

        private void ClearBeforeClose()
        {
            _currentSong = null;
            _artists.Clear();

            ActionSubmit = null;
        }

        private void btnAddArtist_Click(object sender, RoutedEventArgs e)
        {
            var currentArtist = cbArtists.SelectedItem as Artist;
            if (currentArtist != null && _currentSongArtits.FirstOrDefault(p => p.Id == currentArtist.Id) == null)
            {
                _currentSongArtits.Add(currentArtist);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedArtist = DGActor.SelectedItem as Artist;
            try
            {
                if (selectedArtist != null)
                {
                    _currentSongArtits.Remove(selectedArtist);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot delete Because {ex.Message}");
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            DiscloseMediaPlayer();
            var imageUrl = await UploadImageHandler();
            if (imageUrl == string.Empty)
                return;

            var songUrl = await UploadSongHandler();
            if (songUrl == string.Empty)
                return;
            txbSongUrl.Text = songUrl;
            _currentSong.SongURL = songUrl;
            _currentSong.CoverImage = imageUrl;

            if (BindingToSongModel())
            {
                ActionSubmit?.Execute(_currentSong);
            }
        }


        private bool BindingToSongModel()
        {
            try
            {
                if (txbSongName.Text == null
                || _currentSongArtits == null || _currentSongArtits.Count < 1
                  || dpDateRealease.Text == null)
                {
                    throw new Exception("Null reference");
                }
                _currentSong.Title = txbSongName.Text;
                _currentSong.Artists = _currentSongArtits.ToList();
                _currentSong.ReleaseDate = DateTime.Parse(dpDateRealease.Text);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Form not filled yet");
                return false;
            }
        }

        #region Song Mp3 File handler
        private DispatcherTimer dispatcherTimer;
        private bool isPlaying;
        private bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
                if (isPlaying)
                {
                    _MediaPlayer?.Play();
                    symbolPlay.Symbol = Wpf.Ui.Common.SymbolRegular.Pause16;
                }
                else
                {
                    _MediaPlayer?.Pause();
                    symbolPlay.Symbol = Wpf.Ui.Common.SymbolRegular.Play16;

                }
            }
        }
        
        private MediaPlayer _mediaPlayer;
        private MediaPlayer _MediaPlayer
        {
            set { _mediaPlayer = value; }
            get { return _mediaPlayer; }
        }


        private void btnBrowseSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image Files(*.mp3, *.mp4) | *.mp3; *.mp4";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string normalizeFilePath = Uri.UnescapeDataString(dlg.FileName);
                txbSongUrl.Text = normalizeFilePath;
                DiscloseMediaPlayer();
                InitSong();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txbSongUrl.Text = _currentSong.SongURL; 
            currentImageSourceType = ImageSourceType.Url;
        }

        private void sliderMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_MediaPlayer != null)
            {
                _MediaPlayer.Position = TimeSpan.FromSeconds(sliderMusic.Value);
            }
        }


        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsPlaying = !IsPlaying;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void InitSong()
        {
            try
            {
                if(_MediaPlayer != null)
                {
                    DiscloseMediaPlayer();
                }
                _MediaPlayer = new MediaPlayer();
                _MediaPlayer.MediaOpened += TrackingMusic;
                _MediaPlayer.Open(new Uri(txbSongUrl.Text));
                //RefreshSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DiscloseMediaPlayer()
        {
            if (_MediaPlayer != null)
            {
                IsPlaying = false;
                dispatcherTimer.Stop();
                _MediaPlayer.Pause();
                _MediaPlayer.Close();
            }
        }


        private void InitTimerDispatch()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += TimerTickMedia;
        }

        private void TimerTickMedia(object? sender, EventArgs e)
        {
            if (_MediaPlayer != null)
            {
                sliderMusic.Value = _MediaPlayer.Position.TotalSeconds;
            }
        }

        private void TrackingMusic(object? sender, EventArgs e)
        {
            if (_MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                sliderMusic.Maximum = _MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                dispatcherTimer.Start();
                sliderMusic.Value = 0;
            }
        }


        private async Task<string> UploadSongHandler()
        {
            try
            {
                var urlLoaded = await UploadSongToFirebase();
                if (urlLoaded != string.Empty)
                {
                    return urlLoaded;
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }

        private async Task<string> UploadSongToFirebase()
        {
            string urlLoad = string.Empty;
            try
            {
                var validateUri = new Uri(txbSongUrl.Text);
                if (validateUri.IsFile)
                {
                    using(var stream = File.Open(validateUri.LocalPath, FileMode.Open))
                    {
                        var firebaseStorage = new FirebaseStorage(Config.Config.FIREBASE_STORAGE);
                        urlLoad = await firebaseStorage
                            .Child($"{Config.Config.FIREBASE_SONG_MP3_FOLDER}/{txbSongName.Text}{DateTime.Now.ToString("yyyyMMddHHmmss")}.mp3")
                            .PutAsync(stream);

                        return urlLoad;
                    }
                }
                else
                {
                    return txbSongUrl.Text;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return urlLoad;
            }
        }


        private async Task<string> UploadImageHandler()
        {
            try
            {
                if (currentImageSourceType == ImageSourceType.File)
                {
                    var urlUploaded = await UpdateImageToFirebase();
                    if (urlUploaded != string.Empty)
                    {
                        return urlUploaded;
                    }
                    return string.Empty;
                }
                else if(currentImageSourceType == ImageSourceType.Url)
                {
                    return _currentSong.SongURL;
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }

        private async Task<string> UpdateImageToFirebase()
        {
            try
            {
                if (imgSong.Source != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var imgSource = imgSong.Source as BitmapSource;

                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(imgSource));
                        encoder.Save(ms);

                        ms.Seek(0, SeekOrigin.Begin);

                        var urlUpload = await new FirebaseStorage(Config.Config.FIREBASE_STORAGE)
                        .Child($"{Config.Config.FIREBASE_SONG_IMG_FOLDER}/{txbSongName.Text}{DateTime.Now.ToString("yyyyMMddHHmmss")}.png")
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


        private void btnRefreshSong_Click(object sender, RoutedEventArgs e)
        {
            RefreshSong();
        }

        private void RefreshSong()
        {
            txbSongUrl.Text = _currentSong.SongURL;
        }
        #endregion

        #region Image Handler
        private ImageSourceType currentImageSourceType = ImageSourceType.None;

        private void btnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image Files(*.jpg, *.png) | *.jpg; *.png";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Bitmap bitmap = new Bitmap(dlg.FileName);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(dlg.FileName);
                bitmapImage.EndInit();

                imgSong.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.File;
            }
        }
        private void InitImage()
        {
            RefreshImage();
        }

        private void btnRefreshImage_Click(object sender, RoutedEventArgs e)
        {
            RefreshImage();
        }

        private void RefreshImage()
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(_currentSong.CoverImage));
                imgSong.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.Url;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void _this_Closed(object sender, EventArgs e)
        {
            DiscloseMediaPlayer();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper hepler = new WindowInteropHelper(this);
            SendMessage(hepler.Handle, 161, 2, 0);
        }
    }
}
