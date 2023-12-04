
using MusicManager.Model;
using MusicManager.View.SubView;
using MusicManager.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace MusicManager.View
{
    /// <summary>
    /// Interaction logic for ArtistManagementView.xaml
    /// </summary>
    public partial class ArtistManagementView : UserControl
    {
        public ArtistManagementView()
        {
            InitializeComponent();
        }

      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ArtistManagementViewModel viewModel)
            {
                viewModel.LoadArtistFromServerCommand.Execute(null);
            }
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Call window
            try
            {
                if(DataContext is ArtistManagementViewModel viewModel)
                {
                    var songs = await App.RepositoryManager.RepoSongs.GetSongs();
                    FormEditDataArtist formEditDataArtist = new FormEditDataArtist(songs, viewModel.CreateArtistCommand);
                    formEditDataArtist.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is ArtistManagementViewModel viewModel)
                {
                    var currentArtist = DGArtist.CurrentItem as Artist;
                    if (currentArtist != null)
                    {
                        var artistFetchSong = await App.RepositoryManager.RepoArtistes.GetArtistWithSongsAsync(currentArtist);
                        var songs = await App.RepositoryManager.RepoSongs.GetSongs();
                        FormEditDataArtist formEditDataArtist = new FormEditDataArtist(artistFetchSong, songs, viewModel.UpdateArtistCommand);
                        formEditDataArtist.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Artisst currently select is null");
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
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
