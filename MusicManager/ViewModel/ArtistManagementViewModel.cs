using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace MusicManager.ViewModel
{
    internal class ArtistManagementViewModel : ViewModelBase
    {
        private ObservableCollection<Artist> _artists;
        public ObservableCollection<Artist> Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                _artists = value;
                OnPropertyChanged(nameof(Artists));
            }
        }
        //private ObservableCollection<Song> _songs;
        //public ObservableCollection<Song> Songs
        //{
        //    get
        //    {
        //        return _songs;
        //    }
        //    set
        //    {
        //        _songs = value;
        //        OnPropertyChanged(nameof(Song));
        //    }
        //}

        public ICommand LoadArtistFromServerCommand { set; get; }
        //public ICommand LoadSongFromServerCommand { set; get; }
        public ICommand CreateArtistCommand { set; get; }    
        public ICommand UpdateArtistCommand { set; get; }
        public ICommand DeleteArtistCommand { set; get; }
    
        public ArtistManagementViewModel()
        {
            LoadArtistFromServerCommand = new ViewModelCommand(PreLoadArtistFromServer);
            //LoadSongFromServerCommand = new ViewModelCommand(PreLoadSongFromServerCommand);
            CreateArtistCommand = new ViewModelCommand(PreCreateArtistCommand);
            UpdateArtistCommand = new ViewModelCommand(PreUpdateArtistCommand);
            DeleteArtistCommand = new ViewModelCommand(PreDeleteArtistCommand);
        }

        private void PreDeleteArtistCommand(object obj)
        {
            if(obj is Artist artist)
            {
                if (MessageBox.Show("Are you sure to delete this Artist, you can't revert it if you click Yes",
                  "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteSongFromServer(artist);
                }
                DeleteSongFromServer(artist);
            }
            else
            {
                MessageBox.Show("Artist is not selected");
            }
        }

        private void PreUpdateArtistCommand(object obj)
        {
            if(obj is Artist artist)
            {
                UpdateArtist(artist);
            }
            else
            {
                MessageBox.Show("Artist is not selected");
            }
        }

        private void PreCreateArtistCommand(object obj)
        {
            if(obj is Artist artist)
            {
                artist.Image = "https://m.media-amazon.com/images/I/51WHgHxF5YL._AC_UF1000,1000_QL80_.jpg";
                artist.Type = "Artist";
                CreateArtist(artist);
            }
            else
            {
                MessageBox.Show("Artist is not selected");
            }
        }
        //private void PreLoadSongFromServerCommand(object obj)
        //{
        //    LoadSongFromServer();
        //}


        private void PreLoadArtistFromServer(object obj)
        {
            LoadArtistFromServer();
        }

        private async void LoadArtistFromServer()
        {
            try
            {
                Artists = new ObservableCollection<Artist>(await App.RepositoryManager.RepoArtistes.GetArtistsAsync());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //private async void LoadSongFromServer()
        //{
        //    try
        //    {
        //        _songs =  new ObservableCollection<Song>(await App.RepositoryManager.RepoSongs.GetSongs());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void CreateArtist(Artist artist)
        {
            try
            {
                App.RepositoryManager.RepoArtistes.AddArtist(artist);
            }   
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void DeleteSongFromServer(Artist artist)
        {
            try
            {
                App.RepositoryManager.RepoArtistes.DeleteArtist(artist);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void UpdateArtist(Artist artist)
        {
            try
            {
                App.RepositoryManager.RepoArtistes.UpdateArtist(artist);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
