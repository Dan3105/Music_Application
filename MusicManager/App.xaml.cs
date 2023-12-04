// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicManager.Client;
using MusicManager.Model;
using MusicManager.Repsitory;
using MusicManager.Services;
using MusicManager.View;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace MusicManager
{

    public partial class App : Application
    {
        //User binding
        public static AuthenticateModel AuthenticateModel { get; set; }

        //Data handler 
        private static IFirebaseService _firebaseService;
        public static IFirebaseService FirebaseService
        {
            get
            {
                if( _firebaseService == null )
                    _firebaseService = new FirebaseService();
                return _firebaseService;
            }
        }
        public static RepositoryManager RepositoryManager ;
            
        //Window UI handler
        public static Window currentWindow;
        public static Action<Song> InvokePlayMusic;
        public static Action DiscloseMediaPlayer;

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            RepositoryManager = new RepositoryManager();

            //var mainView = new MainView();
            //mainView.Show();
            var loginView = new LoginView();
            currentWindow = loginView;
            currentWindow.Show();

        }

        public static void Signout()
        {
            AuthenticateModel = null;
            DiscloseMediaPlayer?.Invoke();

            Axios.Client.DefaultRequestHeaders.Clear();
            currentWindow.Close();
            currentWindow = new LoginView();
            currentWindow.Show();
        }
    }
}
