using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Youtube.Utility.Service;
using Youtube.Views;
using YoutubeAPI;

namespace Youtube
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public static INavigationService NavigationService { get; set; }
        public static string ChannelId { get; set; }
        public static string ChannelName { get; set; }
        public static string ChannelImg { get; set; }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            YoutubeAPI.Auth.Token token = new YoutubeAPI.Auth.Token();
            while (!await token.IsValidScope())
            {
                MessageBox.Show("請完整授權");
                await token.ReGetTokenByCode();
            }
            YoutubeContext youtubeContext = new YoutubeContext();
            var channel = await youtubeContext.Channel.GetMyChannelAsync();
            ChannelId = channel.items[0].id;
            ChannelName = channel.items[0].snippet.title;
            ChannelImg = channel.items[0].snippet.thumbnails.medium.url;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
