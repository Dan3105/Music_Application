using HandyControl.Controls;
using MusicManager.Model;
using MusicManager.View.SubView;
using MusicManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
namespace MusicManager.View
{
    /// <summary>
    /// Interaction logic for SongManagementView.xaml
    /// </summary>
    public partial class SongManagementView : UserControl
    {

        public SongManagementView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongManagementViewModel viewModel)
            {
                viewModel.SongRetrieveCommand.Execute(null);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongManagementViewModel viewModel)
            {
                Song song = this.DGMusics.SelectedItem as Song;
                viewModel.DeleteSongCommand.Execute(song);
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is SongManagementViewModel viewModel)
                {
                    var song = this.DGMusics.SelectedItem as Song;
                    if (song != null)
                    {
                        var songFromDB = await App.RepositoryManager.RepoSongs.GetSong(song.Id);
                        var artists = await App.RepositoryManager.RepoArtistes.GetArtistsAsync();
                        FormEditDataSong formSongEdit = new FormEditDataSong(artists, viewModel.SubmitEditSongCommand, songFromDB);
                        formSongEdit.ShowDialog();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("song is null");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongManagementViewModel viewModel)
            {
                var artists = await App.RepositoryManager.RepoArtistes.GetArtistsAsync();
                FormEditDataSong formSongEdit = new FormEditDataSong(artists, viewModel.SubmitAddSongCommand);
                formSongEdit.Show();
            }
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(DataContext is SongManagementViewModel viewModel)
            {
                try
                {
                    var textSearch = TBoxSearch.Text;
                    var songs = viewModel.SongLists.Where(p => p.Title.Contains(textSearch)
                                                    || string.Join(" ",p.Artists.Select(p => p.Name).ToList()).Contains(textSearch)
                                                    || p.ReleaseDate.ToString().Contains(textSearch)
                                                    || p.Likes.ToString().Contains(textSearch)
                                                    || p.SongURL.ToString().Contains(textSearch)
                                                    || p.Id.ToString().Contains(textSearch)
                                                    );
                    DGMusics.ItemsSource = songs;
                }
                catch(Exception ex)
                {
                    DGMusics.ItemsSource = viewModel.SongLists;
                }
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is SongManagementViewModel viewModel)
            {
                var song = DGMusics.SelectedItem as Song;
                if(song != null)
                {
                    viewModel.PlaySongCommand.Execute(song);
                }
            }
        }
    }
}
