using MusicManager.Model;
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
using System.Windows.Shapes;

namespace MusicManager.View.SubView
{
    /// <summary>
    /// Interaction logic for MiniDGMusicSelect.xaml
    /// </summary>
    public partial class MiniDGMusicSelect : Window
    {
        public List<SongSelectors> songs { get; set; }

        public MiniDGMusicSelect()
        {
            InitializeComponent();
        }
        private ObservableCollection<SongSelectors> _selectorList { set; get; }
        private Action<IEnumerable<Song>> actionAterPressSubmit;
        public MiniDGMusicSelect(IEnumerable<Song> songs, IEnumerable<Song> songCurrentlySelected, Action<IEnumerable<Song>> action)
        {
            InitializeComponent();
            _selectorList = new ObservableCollection<SongSelectors>();
            foreach(var asong in  songs)
            {
                bool _isSelected = false;
                if (songCurrentlySelected.FirstOrDefault(p => p.Id == asong.Id) != null)
                    _isSelected = true;

                _selectorList.Add(new SongSelectors
                {
                    song = asong,
                    isSelected = _isSelected
                });
            }
            this.DGMiniSongList.ItemsSource = _selectorList;
            actionAterPressSubmit = action;
        }

        private IEnumerable<Song> GetSelectedList()
        {
            return _selectorList.Where(s => s.isSelected).Select(p => p.song);
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            actionAterPressSubmit?.Invoke(GetSelectedList());
            ActionBeforeClose();
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ActionBeforeClose();
            this.Close();
        }

        private void ActionBeforeClose()
        {
            _selectorList.Clear();
            actionAterPressSubmit = null;
        }
    }
}
