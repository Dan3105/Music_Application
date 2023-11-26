using MusicManager.Client;
using MusicManager.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        //Fields
        private string? _username;
        private string? _password;
        private string? _errorMessage;
        private bool _isViewVisible = true;

        public string Username { 
            get => _username; 
            set{
                _username = value;
                ErrorMessage = "";
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password { 
            get => _password; 
            set {
                _password = value;
                ErrorMessage = "";
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage { 
            get => _errorMessage; 
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }

        }
        public bool IsViewVisible { 
            get => _isViewVisible; 
            set 
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //-Commands
        public ICommand LoginCommand { get; }
        public ICommand ShowPasswordCommand { get; }


        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            ShowPasswordCommand = new ViewModelCommand(ExecuteShowPasswordCommand);
        }

        private void ExecuteShowPasswordCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            if ((string.IsNullOrEmpty(Username) || Username.Length < 3) ||
                (Password == null || Password.Length < 3))
            {
                return false;
            }
            return true;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            var AuthModel = await App.RepositoryManager.RepoAuthentication.Authenticate(Username, Password);
            if(AuthModel == null)
            {
                ErrorMessage = "Error in Logging check password and email";
                return;
            }

            //Thread.CurrentPrincipal =  new GenericPrincipal(new GenericIdentity(AuthModel), null);
            App.AuthenticateModel = AuthModel;
            Axios.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {App.AuthenticateModel.AccessToken.Token}");
            IsViewVisible = false;
            MainView mainWindow = new MainView();
            mainWindow.Show();
        }
    }
}
