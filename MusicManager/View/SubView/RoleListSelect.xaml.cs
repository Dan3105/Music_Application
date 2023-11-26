using MusicManager.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RoleListSelect.xaml
    /// </summary>
    public partial class RoleListSelect : Window
    {
        private List<Role> _roleSelection { set; get; }
        private Role currentRole = null;
        private Action<Role> _updateRole;
        public RoleListSelect()
        {
            InitializeComponent();
        }
        public RoleListSelect(IEnumerable<Role> roles, Action<Role> updateRole, Role currentSelectRole=null)
        {
            _roleSelection = roles.ToList();
            _updateRole = updateRole;
            InitializeComponent();
            if(currentSelectRole != null)
            {
                currentRole = currentSelectRole;   
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RoleList.ItemsSource = _roleSelection;
            if(currentRole != null)
            {
                this.RoleList.SelectedItem = _roleSelection.Find(p => p.Id == currentRole.Id);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(this.RoleList.SelectedItem is Role role)
                {
                    _updateRole?.Invoke(role);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                _updateRole = null;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _updateRole = null;
            this.Close();
        }
    }
}
