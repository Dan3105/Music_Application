using CommunityToolkit.Mvvm.Input;
using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager.ViewModel
{
    internal class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<Artist> artists;
        public ObservableCollection<Artist> Artists
        {
            get
            {
                return artists;
            }
            set
            {
                artists = value;
                OnPropertyChanged(nameof(Artists));
            }
        }

        private ObservableCollection<Song> songReleases;
        public ObservableCollection<Song> SongReleases
        {
            get
            {
                return songReleases;
            }
            set
            {
                songReleases = value;
                OnPropertyChanged(nameof(SongReleases));
            }
        }

        public ICommand GetListArtistCommand;
        public ICommand GetReleasesSongsCommand;
        public ICommand PlayASong;
        public HomeViewModel()
        {
            GetListArtistCommand = new ViewModelCommand(GetListArtists);
            GetReleasesSongsCommand = new ViewModelCommand(GetReleasesSongs);
            PlayASong = new RelayCommand<object>(AssignSongToPlay);
        }

        private void AssignSongToPlay(object? obj)
        {
            if(obj is Song song)
            {
                App.InvokePlayMusic?.Invoke(song);
            }
        }

        private async void GetReleasesSongs(object obj)
        {
            try
            {
                SongReleases = new ObservableCollection<Song>((await App.RepositoryManager.RepoSongs.GetSongs()).ToList());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SongReleases = new ObservableCollection<Song>();
            }
        }

        private async void GetListArtists(object obj)
        {
            try
            {
                Artists = new ObservableCollection<Artist>((await App.RepositoryManager.RepoArtistes.GetArtistsAsync()).ToList());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Artists = new ObservableCollection<Artist>();
            }
        }
    }
}
