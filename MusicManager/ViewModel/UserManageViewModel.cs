using MusicManager.Model;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace MusicManager.ViewModel
{
    class UserManageViewModel : ViewModelBase
    {
        public ICommand LoadDataFromServerCommand { get; set; }
        public ICommand SubmitEditFormCommand { get; set; }

        #region Database Container
        private ObservableCollection<Model.User> users;
        public ObservableCollection<Model.User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private Model.User selectedUser;
        public Model.User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        #endregion

        public UserManageViewModel() {
            LoadDataFromServerCommand = new ViewModelCommand(LoadDataFromServer);
            SubmitEditFormCommand = new ViewModelCommand(SubmitToServer);
        }

        private void SubmitToServer(object obj)
        {
            if (obj is User user)
            {
                PatchUserToServer(user);
            }
            else
            {
                MessageBox.Show("Object is not user");
            }
        }

        private void LoadDataFromServer(object obj)
        {
            GetListUsersFromServer();
        }

        #region Database Retrieve Handler
        private async void GetListUsersFromServer()
        {
            try
            {
                Users = new ObservableCollection<Model.User>((await App.RepositoryManager.RepoUsers.GetUsers()).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Users = new ObservableCollection<Model.User>();
            }
        }
        private async void PatchUserToServer(Model.User user)
        {
            try
            {
                if(await App.RepositoryManager.RepoUsers.PatchUser(user))
                {
                    MessageBox.Show("Update user successfully");
                    LoadDataFromServerCommand?.Execute(null);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
