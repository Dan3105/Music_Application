using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static MusicManager.Config.Config;
using MessageBox = System.Windows.MessageBox;
namespace MusicManager.View.SubView
{
    /// <summary>
    /// Interaction logic for FormEditDataAlbum.xaml
    /// </summary>
    public partial class FormEditDataAlbum : Window
    {
        private ImageSourceType currentImageSourceType = ImageSourceType.None;
        private ObservableCollection<Song> _allSongsParsing;
        private ObservableCollection<Song> _currentSongContains;
        private ObservableCollection<Artist> _allArtistParsing;
        private Album _album;
        private ICommand SubmitCommand;
        public FormEditDataAlbum(IEnumerable<Song> songs, IEnumerable<Artist> artist, ICommand submitCommand)
        {
            InitializeComponent();
            _album = new Album();
            _allSongsParsing = new ObservableCollection<Song>(songs);
            _allArtistParsing = new ObservableCollection<Artist>(artist);
            SubmitCommand = submitCommand;

            BindingUI();
        }

        public FormEditDataAlbum(Album album, IEnumerable<Song> songs, IEnumerable<Artist> artist, ICommand submitCommand)
        {
            InitializeComponent();
            _album = album;
            _allSongsParsing = new ObservableCollection<Song>(songs);
            _allArtistParsing = new ObservableCollection<Artist>(artist);
            SubmitCommand = submitCommand;

            BindingUI();
        }

        private void BindingUI()
        {
            txbAlbumName.Text = _album.Name;
            dpDateRealease.Text = _album.ReleaseDate.ToString();
            if(_album.Songs?.Count > 0)
            {
                _currentSongContains = new ObservableCollection<Song>(_album.Songs);
            }
            this.DGSong.ItemsSource = _currentSongContains;
            this.cbArtists.ItemsSource = _allArtistParsing;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var result = _currentSongContains.Where(s => s.Title.Contains(TBoxSearch.Text)).ToList();
                DGSong.ItemsSource = result;
            }
            catch
            {
                DGSong.ItemsSource = _currentSongContains;
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

        private void CommandAfterCloseMiniDGMusicSelect(IEnumerable<Song> SongSelected)
        {
            this._currentSongContains = new ObservableCollection<Song>(SongSelected);
            DGSong.ItemsSource = _currentSongContains;

        }

        private void btnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image Files(*.jpg, *.png, *.jpeg) | *.jpg; *.png; *.jpeg";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Bitmap bitmap = new Bitmap(dlg.FileName);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(dlg.FileName);
                bitmapImage.EndInit();

                imgAlbum.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.File;
            }
        }

        private void btnRefreshImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(_album.ImageUrl));
                imgAlbum.Source = bitmapImage;
                currentImageSourceType = ImageSourceType.Url;
            }
            catch (Exception ex)
            {
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper hepler = new WindowInteropHelper(this);
            SendMessage(hepler.Handle, 161, 2, 0);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BindingToModelArtist())
                {
                    
                    if (currentImageSourceType == ImageSourceType.File)
                    {
                        object param = new Tuple<object, Album>(imgAlbum.Source, _album);
                        SubmitCommand?.Execute(param);
                    }
                    else if (currentImageSourceType == ImageSourceType.Url)
                    {
                        object param = new Tuple<object, Album>(_album.ImageUrl, _album);
                        SubmitCommand?.Execute(param);

                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Submit Got Error, Do you want to close this Form", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private bool BindingToModelArtist()
        {
            try
            {

                if (!ValidateBinidng())
                {
                    return false;
                }
                _album.Name = txbAlbumName.Text;
                _album.ReleaseDate = DateTime.Parse(dpDateRealease.Text);
                _album.Artiste = cbArtists.SelectedItem as Artist;
                _album.Songs = _currentSongContains.ToList();
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateBinidng()
        {
            if (currentImageSourceType == ImageSourceType.None)
            {
                MessageBox.Show("Image is not Selected!");
                return false;
            }
            if (txbAlbumName.Text.Length < 1)
            {
                MessageBox.Show("Name Artist is not filled!");
                return false;
            }
            if(dpDateRealease.Text.Length < 1)
            {
                MessageBox.Show("Date Release is not set");
                return false;
            }
            if(cbArtists.SelectedItem is not Artist)
            {
                MessageBox.Show("Artist is not selected");
                return false;
            }
            return true;
        }
    }
}
