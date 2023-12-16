using Firebase.Storage;
using MusicManager.Model;
using MusicManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicManager.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void listSongs_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.HorizontalChange);
            e.Handled = true;
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(this.listArtists);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeViewModel homeViewModel)
            {
                homeViewModel.GetReleasesSongsCommand.Execute(null);
                homeViewModel.GetListArtistCommand.Execute(null);
            }
        }

        private void listSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is HomeViewModel homeViewModel)
            {
                Song song = listSongs.SelectedItem as Song;
                if (song != null)
                {
                    homeViewModel.PlayASong.Execute(song);
                }
            }
        }

        private async void test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = "https://firebasestorage.googleapis.com/v0/b/musicproject-2737f.appspot.com/o/mp3-song%2Fworldsmallestviolin20231206165237.mp3?alt=media&token=4fb1f740-a119-45e5-95d5-255fe89ffc61";
                Uri newUri = new Uri(url);
                string path = newUri.LocalPath;
                path = Uri.UnescapeDataString(path);
                int indexOfLastSlash = path.LastIndexOf('/');
                string desiredPath = path.Substring(indexOfLastSlash + 1);
                MessageBox.Show(desiredPath);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
