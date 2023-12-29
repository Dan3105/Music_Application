using MusicManager.Model;
using MusicManager.View.SubView;
using MusicManager.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace MusicManager.View
{
    /// <summary>
    /// Interaction logic for AlbumManagementView.xaml
    /// </summary>
    public partial class AlbumManagementView : UserControl
    {
        public AlbumManagementView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AlbumManagementViewModel viewModel)
            {
                viewModel.LoadAlbumsFromServerCommand.Execute(null);
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is AlbumManagementViewModel viewModel)
                {
                    var artists = await App.RepositoryManager.RepoArtistes.GetArtistsAsync();
                    FormEditDataAlbum formEditDataAlbum = new FormEditDataAlbum(artists, viewModel.CreateAlbumCommand);
                    formEditDataAlbum.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is AlbumManagementViewModel viewModel)
            {
                try
                {
                    var result = viewModel.Albums.Where(x => x.Name.Contains(TBoxSearch.Text) ||
                                        x.Artiste.Name.Contains(TBoxSearch.Text));
                    DGAlbum.ItemsSource = result;
                }
                catch
                {
                    DGAlbum.ItemsSource = viewModel.Albums;
                }
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is AlbumManagementViewModel viewModel)
                {
                    var currentAlbum = DGAlbum.CurrentItem as Album;
                    if (currentAlbum != null)
                    {
                        var artists = await App.RepositoryManager.RepoArtistes.GetArtistsAsync();
                        FormEditDataAlbum formEditDataAlbum = new FormEditDataAlbum(currentAlbum, artists, viewModel.UpdateAlbumCommand);
                        formEditDataAlbum.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Album currently select is null");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is AlbumManagementViewModel viewModel)
                {
                    var currentArtist = DGAlbum.CurrentItem as Album;
                    if (currentArtist != null)
                    {
                        viewModel.DeleteAlbumCommand?.Execute(currentArtist);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
