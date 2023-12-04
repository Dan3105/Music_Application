using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace MusicManager.ViewModel
{
    internal class SongManagementViewModel : ViewModelBase
    {
        public ICommand SongRetrieveCommand;
        public ICommand SubmitEditSongCommand;
        public ICommand SubmitAddSongCommand;
        public ICommand DeleteSongCommand;
        public ICommand PlaySongCommand;
        private ObservableCollection<Song> songLists;
        public ObservableCollection<Song> SongLists { 
            get { 
                return songLists; 
            } 
            set
            {
                songLists = value;
                OnPropertyChanged(nameof(SongLists));
            }
        }
    
        public SongManagementViewModel()
        {
            SongRetrieveCommand = new ViewModelCommand(PreRetrieveSongFromServer);
            SubmitEditSongCommand = new ViewModelCommand(PreSubmitSongToServer);
            SubmitAddSongCommand = new ViewModelCommand(PreAddSongToServer);
            DeleteSongCommand = new ViewModelCommand(PreDeleteSongFromServer);
            PlaySongCommand = new ViewModelCommand(PlaySong);
        }

        private void PlaySong(object obj)
        {
            if(obj is Song song)
            {
                App.InvokePlayMusic?.Invoke(song);
            }
        }

        private void PreDeleteSongFromServer(object obj)
        {
            if(obj is Song song)
            {
                if(MessageBox.Show("Are you sure to delete this Song, you can't revert it if you click Yes", 
                    "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) { 
                    DeleteSongFromServer(song);
                }
            }
            else
            {
                MessageBox.Show("No song is seleted");
            }
        }

        private async void DeleteSongFromServer(Song song)
        {
            await App.RepositoryManager.RepoSongs.DeleteSong(song);
        }

        private void PreAddSongToServer(object obj)
        {
            if (obj is Song song)
            {
                song.Duration = 230;
                song.Likes = 69969;
                PostSongToServer(song);
            }
            else
            {
                MessageBox.Show("Song is null");
            }
        }

        private async void PostSongToServer(Song song)
        {
            if(song.Artists.Count < 1)
            {
                MessageBox.Show("Artist is empty", "Can't resolve", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await App.RepositoryManager.RepoSongs.AddSong(song);
        }

        private void PreSubmitSongToServer(object obj)
        {
            if(obj is Song song)
            {
                PutSongToServer(song);
            }
            else
            {
                MessageBox.Show("Song is null");
            }
        }

        private async void PutSongToServer(Song song)
        {
            await App.RepositoryManager.RepoSongs.UpdateSong(song);
        }

        private void PreRetrieveSongFromServer(object obj)
        {
            RetrieveSongFromServerAsync();
        }

        private async Task RetrieveSongFromServerAsync()
        {
            SongLists = new ObservableCollection<Song>((await App.RepositoryManager.RepoSongs.GetSongs()));
        }
    }
}
