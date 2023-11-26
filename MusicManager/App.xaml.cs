// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicManager.Model;
using MusicManager.Repsitory;
using MusicManager.View;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace MusicManager
{

    public partial class App : Application
    {
        public static AuthenticateModel AuthenticateModel { get; set; }

        public static RepositoryManager RepositoryManager ;
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            RepositoryManager = new RepositoryManager();

            //var mainView = new MainView();
            //mainView.Show();
            var loginView = new LoginView();
            loginView.Show();

        }
    }
}
