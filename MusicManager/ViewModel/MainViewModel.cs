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

        public MainViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowUserManagementViewCommand = new ViewModelCommand(ExecuteManagementViewCommand);
            ExecuteShowHomeViewCommand(null);
        }

        private void ExecuteManagementViewCommand(object obj)
        {
            
            CurrentView = new UserManageViewModel();
            CurrentBreadCrumb = "User Management";
            SymbolIcon = "PeopleList20";
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentView = new HomeViewModel();
            CurrentBreadCrumb = "Home";
            SymbolIcon = "Home12";
        }
    }
}
