using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

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
            SubmitEditSongCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, string mediaPlayer, Song song))
                {
                    PreSubmitSongToServer(image, mediaPlayer, song);
                };
            });
            SubmitAddSongCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, string mediaPlayer, Song song))
                {
                    PreAddSongToServer(image, mediaPlayer, song);
                };
            });
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
            SongRetrieveCommand?.Execute(null);
        }

        private void PreAddSongToServer(object image, string media, Song song)
        {
            if (image == null)
            {
                MessageBox.Show("Image is null");
                return;
            }
            if (media == null)
            {
                MessageBox.Show("Media is null");
                return;
            }
            if (song == null)
            {
                MessageBox.Show("Song is null");
                return;
            }

            PostSongToServer(image, media, song);

        }

        private async void PostSongToServer(object image, string media, Song song)
        {
            if(song.Artists.Count < 1)
            {
                MessageBox.Show("Artist is empty", "Can't resolve", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await App.RepositoryManager.RepoSongs.AddSong(image, media, song);
            SongRetrieveCommand?.Execute(null);

        }

        private void PreSubmitSongToServer(object image, string media, Song song)
        {
            if (image == null)
            {
                MessageBox.Show("Image is null");
                return;
            }
            if (media == null)
            {
                MessageBox.Show("Media is null");
                return;
            }
            if (song == null)
            {
                MessageBox.Show("Song is null");
                return;
            }
            PutSongToServer(image, media, song);

        }

        private async void PutSongToServer(object image, string media, Song song)
        {
            await App.RepositoryManager.RepoSongs.UpdateSong(image, media, song);
            SongRetrieveCommand?.Execute(null);
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
