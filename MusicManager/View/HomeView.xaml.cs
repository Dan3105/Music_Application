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
    }
}
