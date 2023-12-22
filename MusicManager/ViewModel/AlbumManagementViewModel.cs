using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MusicManager.ViewModel
{
    internal class AlbumManagementViewModel : ViewModelBase
    {
        private ObservableCollection<Album> _albums;
        public ObservableCollection<Album> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                _albums = value;
                OnPropertyChanged(nameof(Albums));
            }
        }


        public ICommand LoadAlbumsFromServerCommand { set; get; }
        public ICommand CreateAlbumCommand { set; get; }
        public ICommand UpdateAlbumCommand { set; get; }
        public ICommand DeleteAlbumCommand { set; get; }

        public AlbumManagementViewModel()
        {
            LoadAlbumsFromServerCommand = new ViewModelCommand(PreLoadAlbumFromServer);
            CreateAlbumCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, Album album))
                {
                    PreCreateAlbumCommand(image, album);
                };
            });

            UpdateAlbumCommand = new ViewModelCommand(parameter =>
            {
                if (parameter is (object image, Album album))
                {
                    PreUpdateAlbumCommand(image, album);
                };
            });

            DeleteAlbumCommand = new ViewModelCommand(PreDeleteAlbumCommand);
        }

        private void PreDeleteAlbumCommand(object obj)
        {
            if (obj is Album album)
            {
                if (MessageBox.Show("Are you sure to delete this Artist, you can't revert it if you click Yes",
                  "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteAlbumFromServer(album);
                }
            }
            else
            {
                MessageBox.Show("Artist is not selected");
            }
        }

        private async void DeleteAlbumFromServer(Album album)
        {
            try
            {
                await App.RepositoryManager.RepoAlbums.DeleteAlbum(album);
                LoadAlbumsFromServerCommand?.Execute(null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void PreUpdateAlbumCommand(object image, Album album)
        {
            if (image == null)
            {
                MessageBox.Show("Image is not selected");
                return;
            }

            if (album == null)
            {
                MessageBox.Show("Artist is not selected");
                return;
            }
            UpdateAlbum(image, album);
        }

        private async void UpdateAlbum(object image, Album album)
        {
            try
            {
                await App.RepositoryManager.RepoAlbums.UpdateAlbum(image, album);
                LoadAlbumsFromServerCommand?.Execute(null);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void PreCreateAlbumCommand(object image, Album album)
        {
            if (image == null)
            {
                MessageBox.Show("Image is not selected");
                return;
            }

            if (album == null)
            {
                MessageBox.Show("Artist is not selected");
                return;
            }
            CreateAlbum(image, album);
        }

        private async void CreateAlbum(object image, Album album)
        {
            try
            {
                await App.RepositoryManager.RepoAlbums.AddAlbum(image, album);
                LoadAlbumsFromServerCommand?.Execute(null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void PreLoadAlbumFromServer(object obj)
        {
            LoadAlbumsFromServer();
        }

        private async void LoadAlbumsFromServer()
        {
            try
            {
                Albums = new ObservableCollection<Album>(await App.RepositoryManager.RepoAlbums.GetAlbums());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
