using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.ViewModel
{
    class UserManageViewModel : ViewModelBase
    {
        public UserManageViewModel() { }
        public class fUser
        {
            public string Email { get; set; }
            public DateTime Created { get; set; }
            public bool IsActive { get; set; }
        }

        public ObservableCollection<fUser> FakeUsers { get; set; } = new ObservableCollection<fUser>
        {
            new fUser
            {
                Email = "user1@example.com",
                Created = DateTime.Now.AddMonths(-3),
                IsActive = true
            },
            new fUser
            {
                Email = "user2@example.com",
                Created = DateTime.Now.AddMonths(-5),
                IsActive = false
            },
            new fUser
            {
                Email = "user3@example.com",
                Created = DateTime.Now.AddMonths(-2),
                IsActive = true
            },
            new fUser
            {
                Email = "user4@example.com",
                Created = DateTime.Now.AddMonths(-7),
                IsActive = false
            },
            new fUser
            {
                Email = "user5@example.com",
                Created = DateTime.Now.AddMonths(-1),
                IsActive = true
            }
        };
    }
}
