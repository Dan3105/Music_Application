using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace MusicManager.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        //List Of All Views
        private List<ViewModelBase> viewModelBases = new List<ViewModelBase>();

        private string _currentBreadCrumb;
        public string CurrentBreadCrumb
        {
            get
            {
                return _currentBreadCrumb;
            }
            set
            {
                _currentBreadCrumb = value;
                OnPropertyChanged(nameof(CurrentBreadCrumb));
            }
        }

        private string _symbolIcon;
        public string SymbolIcon
        {
            get
            {
                return _symbolIcon;
            }
            set
            {
                _symbolIcon = value;
                OnPropertyChanged(nameof(SymbolIcon));
            }
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView { 
            get 
            { 
                return _currentView; 
            }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

       

        public ICommand ShowHomeViewCommand { get; set; }
        public ICommand ShowUserManagementViewCommand { get; set; }
        public ICommand ShowSongManagementViewCommand { get; set; }
        public ICommand ShowArtistManagementViewCommand { get; set; }

        public MainViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowUserManagementViewCommand = new ViewModelCommand(ExecuteUserManagementViewCommand);
            ShowSongManagementViewCommand = new ViewModelCommand(ExecuteSongManagementViewCommand);
            ShowArtistManagementViewCommand = new ViewModelCommand(ExecuteArtistManagementViewCommand);
            ExecuteShowHomeViewCommand(null);
        }

        private void ExecuteArtistManagementViewCommand(object obj)
        {
            CurrentView = new ArtistManagementViewModel();
            CurrentBreadCrumb = "Artist Management";
            SymbolIcon = "SlideMicrophone24";
        }

        private void ExecuteUserManagementViewCommand(object obj)
        {
            
            CurrentView = new UserManageViewModel();
            CurrentBreadCrumb = "User Management";
            SymbolIcon = "PeopleList20";
        }

        private void ExecuteSongManagementViewCommand(object obj)
        {
            CurrentView = new SongManagementViewModel();
            CurrentBreadCrumb = "Music Management";
            SymbolIcon = "MusicNote124";
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentView = new HomeViewModel();
            CurrentBreadCrumb = "Home";
            SymbolIcon = "Home12";
        }
    }
}
