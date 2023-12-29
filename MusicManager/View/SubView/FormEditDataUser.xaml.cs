using MusicManager.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MusicManager.View.SubView
{
    /// <summary>
    /// Interaction logic for FormAddDataUser.xaml
    /// </summary>
    public partial class FormEditDataUser : Window
    {
        public ICommand ActionSubmit
        {
            get;
            set;
        }

        private User _refUser;
        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> RoleList
        {
            get { return _roles; }
            set
            {
                _roles = value;
            }
        }

        private ObservableCollection<Role> currentUserRoles;

        public FormEditDataUser()
        {
            InitializeComponent();
        }

        public FormEditDataUser(User user, IEnumerable<Role> roles, ICommand eventSubmit)
        {
            InitializeComponent();
            _refUser = user;
            RoleList = new ObservableCollection<Role>(roles);
            BindingDataToUI(user, eventSubmit);
        }

        private void BindingDataToUI(User user, ICommand eventSubmit)
        {
            this.txbEmail.Text = user.Email;
            this.txbCreated.Text = user.CreatedDate.ToString();
            this.chbActive.IsChecked = user.IsActive;
            this.ActionSubmit = eventSubmit;
            this.currentUserRoles = new ObservableCollection<Role>(user.RoleDTOs);
            this.DGRole.ItemsSource = this.currentUserRoles;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ActionSubmit = null;
            this.Close();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to submit?", "Submit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (_refUser != null && ActionSubmit != null)
                {
                    _refUser.IsActive = this.chbActive.IsChecked ?? false;
                    _refUser.RoleDTOs = this.currentUserRoles.ToList();
                    ActionSubmit.Execute(_refUser);

                }
                else
                {
                    MessageBox.Show("Binding data is null");
                }
                ActionSubmit = null;
                this.Close();
            }
                
        }

        private void AddUserRoleAction(Role role)
        {
            if (role != null && currentUserRoles.FirstOrDefault(r => r.Id ==role.Id) == null)
            {
                currentUserRoles.Add(role);
            }
        }

        private void EditUserRoleAction(Role role)
        {
            if (role == null)
            {
                return;
            }
            try
            {
                var roleCurrentSelected = this.DGRole.SelectedItem as Role;
                var roleExists = currentUserRoles.FirstOrDefault(p => p.Id == role.Id);
                if (roleExists != null && roleExists.Id != roleCurrentSelected.Id)
                {
                    currentUserRoles.Remove(roleCurrentSelected);
                }
                else
                {
                    int index = currentUserRoles.IndexOf(roleCurrentSelected);
                    roleCurrentSelected = role;
                    currentUserRoles[index] = roleCurrentSelected;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BtnAddRole_Click(object sender, RoutedEventArgs e)
        {
            RoleListSelect roleListSelect = new RoleListSelect(RoleList, AddUserRoleAction);
            roleListSelect.Show();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var role = this.DGRole.SelectedItem as Role;
            if (role != null)
            {
                RoleListSelect roleListSelect = new RoleListSelect(RoleList, EditUserRoleAction, role);
                roleListSelect.Show();
            }
            else
            {
                MessageBox.Show("role is null");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var role = this.DGRole.SelectedItem as Role;
            if (role != null)
            {
                currentUserRoles.Remove(role);
            }
            else
            {
                MessageBox.Show("role is null");
            }
        }
    }
}
