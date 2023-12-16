using MusicManager.Model;
using MusicManager.View.SubView;
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
using System.Windows.Shapes;

namespace MusicManager.View
{
    public partial class UserManageView : UserControl
    {
        public UserManageView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManageViewModel userManageViewModel)
            {
                userManageViewModel.LoadDataFromServerCommand.Execute(null);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TBoxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if ( DataContext is UserManageViewModel userManageViewModel)
            {
                var result = userManageViewModel.Users.Where(x => x.Email.Contains(TBoxSearch.Text) || x.CreatedDate.ToString().Contains(TBoxSearch.Text)).ToList();
                DGridCustomer.ItemsSource = result;
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is UserManageViewModel userManageViewModel)
                {
                    var refUser = (User)DGridCustomer.CurrentItem;
                    var userFromDatabase = await App.RepositoryManager.RepoUsers.GetUser(refUser.Id);
                    var roleList = await App.RepositoryManager.RepoUsers.GetRoles();
                    FormEditDataUser editUser = new FormEditDataUser(userFromDatabase, roleList, userManageViewModel.SubmitEditFormCommand);
                    editUser.ShowDialog();

                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
