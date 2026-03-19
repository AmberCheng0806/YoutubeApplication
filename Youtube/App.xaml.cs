using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Youtube.Utility.Service;
using Youtube.Views;

namespace Youtube
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public static INavigationService NavigationService { get; set; }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            YoutubeAPI.Auth.Token token = new YoutubeAPI.Auth.Token();
            while (!await token.IsValidScope())
            {
                MessageBox.Show("請完整授權");
                await token.ReGetTokenByCode();
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
