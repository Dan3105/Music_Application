using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

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

        public ICommand LoadArtistFromServerCommand { set; get; }
        public ICommand CreateArtistCommand { set; get; }    
        public ICommand UpdateArtistCommand { set; get; }
        public ICommand DeleteArtistCommand { set; get; }

        public ArtistManagementViewModel()
        {
            LoadArtistFromServerCommand = new ViewModelCommand(PreLoadArtistFromServer);
            CreateArtistCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, Artist artist))
                {
                    PreCreateArtistCommand(image, artist);
                };
            });

            UpdateArtistCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, Artist artist))
                {
                    PreUpdateArtistCommand(image, artist);
                };
            });

            DeleteArtistCommand = new ViewModelCommand(PreDeleteArtistCommand);
        }

        private void PreDeleteArtistCommand(object obj)
        {
            if(obj is Artist artist)
            {
                if(artist.Songs != null && artist.Songs.Count > 0)
                {
                    MessageBox.Show("Cannot delete artists because they have music in database");
                    return;
                }
                if (MessageBox.Show("Are you sure to delete this Artist, you can't revert it if you click Yes",
                  "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteArtistFromServer(artist);
                }
            }
            else
            {
                MessageBox.Show("Artist is not selected");
            }
        }

        private void PreUpdateArtistCommand(object image, Artist artist)
        {
            if (image == null)
            {
                MessageBox.Show("Image is not selected");
                return;
            }

            if (artist == null)
            {
                MessageBox.Show("Artist is not selected");
                return;
            }
            UpdateArtist(image, artist);

        }

        private void PreCreateArtistCommand(object image, Artist artist)
        {
            if (image == null)
            {
                MessageBox.Show("Image is not selected");
                return;
            }

            if (artist == null)
            {
                MessageBox.Show("Artist is not selected");
                return;
            }
            CreateArtist(image, artist);

        }


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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void CreateArtist(object image, Artist artist)
        {
            try
            {
                await App.RepositoryManager.RepoArtistes.AddArtist(image, artist);
                LoadArtistFromServerCommand?.Execute(null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private async void DeleteArtistFromServer(Artist artist)
        {
            try
            {
                await App.RepositoryManager.RepoArtistes.DeleteArtist(artist);
                LoadArtistFromServerCommand?.Execute(null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void UpdateArtist(object image, Artist artist)
        {
            try
            {
                await App.RepositoryManager.RepoArtistes.UpdateArtist(image, artist);
                LoadArtistFromServerCommand?.Execute(null);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
