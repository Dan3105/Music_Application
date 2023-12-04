using Firebase.Storage;
using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static MusicManager.Config.Config;
using MessageBox = System.Windows.MessageBox;

namespace MusicManager.View.SubView
{
    /// <summary>
    /// Interaction logic for FormEditDataArtist.xaml
    /// </summary>
    public partial class FormEditDataArtist : Window
    {
        private ImageSourceType currentImageSourceType = ImageSourceType.None;

        private ObservableCollection<Song> _allSongsParsing;
        private ObservableCollection<Song> _currentSongContains;
        private Artist _artist;
        private ICommand SubmitCommand;
        public FormEditDataArtist()
        {
            InitializeComponent();
        }

        public FormEditDataArtist(Artist crrArtist, IEnumerable<Song> songs, ICommand submitCommand)
        {
            InitializeComponent();
            _artist = crrArtist;
            _allSongsParsing = new ObservableCollection<Song>(songs);
            SubmitCommand = submitCommand;
            BindingUI(crrArtist);
        }

        public FormEditDataArtist(IEnumerable<Song> songs, ICommand submitCommand)
        {
            InitializeComponent();
            SubmitCommand = submitCommand;
            _artist = new Artist();
            _allSongsParsing = new ObservableCollection<Song>(songs);
            BindingUI(_artist);
        }

        private void BindingUI(Artist artist)
        {
            txbArtistName.Text = artist.Name;
            tbBio.Text = artist.Bio;
            if (artist.Songs == null)
            {
                artist.Songs = new List<Song>();
            }
            _currentSongContains = new ObservableCollection<Song>(artist.Songs);
            this.DGSong.ItemsSource = _currentSongContains;
            RefreshImage();
        }

        #region Handler Button Action UI
        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!ValidateBeforeSubmit())
                {
                    return;
                }
                if (currentImageSourceType == ImageSourceType.File)
                {
                    var urlUploaded = await UpdateImageToFirebase();
                    if (urlUploaded == string.Empty)
                    {
                        return;
                    }
                    _artist.Image = urlUploaded;
                }
                _artist.Name = txbArtistName.Text;
                _artist.Bio = tbBio.Text;
                _artist.Songs = _currentSongContains.ToList();
                SubmitCommand?.Execute(_artist);
                ActionBeforeClose();
                this.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Submit Got Error, Do you want to close this Form", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    ActionBeforeClose();
                    this.Close();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var song = DGSong.CurrentItem as Song;
            if (song != null)
            {
                _currentSongContains.Remove(song);
            }
            else
            {
                MessageBox.Show("Song currently not selected");
            }
        }

        private void btnAddSong_Click(object sender, RoutedEventArgs e)
        {
            MiniDGMusicSelect miniDGMusicSelect = new MiniDGMusicSelect(_allSongsParsing, _currentSongContains, CommandAfterCloseMiniDGMusicSelect);
            miniDGMusicSelect.ShowDialog();
        }
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

                imgArtist.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.File;
            }
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ActionBeforeClose();
            this.Close();
        }
        private void btnRefreshImage_Click(object sender, RoutedEventArgs e)
        {
            RefreshImage();
        }
        private void TBoxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        #endregion

        private void CommandAfterCloseMiniDGMusicSelect(IEnumerable<Song> SongSelected)
        {
            this._currentSongContains = new ObservableCollection<Song>(SongSelected);
            DGSong.ItemsSource = _currentSongContains;

        }
        private void ActionBeforeClose()
        {
            _allSongsParsing.Clear();
            _currentSongContains.Clear();
            _artist = null;
            SubmitCommand = null;
        }

        private void RefreshImage()
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(_artist.Image));
                imgArtist.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.Url;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateBeforeSubmit()
        {
            if (currentImageSourceType == ImageSourceType.None)
            {
                MessageBox.Show("Image is not Selected!");
                return false;
            }
            if (txbArtistName.Text.Length < 0)
            {
                MessageBox.Show("Name Artist is not filled!");
                return false;
            }
            return true;
        }

        private async Task<string> UpdateImageToFirebase()
        {
            return await App.FirebaseService.UpdateDataImageToCloud(imgArtist.Source, txbArtistName.Text, FIREBASE_ARTIST_IMG_FOLDER);
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