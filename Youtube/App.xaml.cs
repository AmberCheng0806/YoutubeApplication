using IoC_Container;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Youtube.Presenters;
using Youtube.Utility.Service;
using Youtube.Views;
using Youtube.Views.Pages.MemberCenterPages;
using Youtube.Views.Pages.VideoPages;
using YoutubeAPI;
using YoutubeAPI.Auth;
using static Youtube.Contracts.MemberCenterPlaylistsContract;
using static Youtube.Contracts.MemberCenterVideosContract;
using static Youtube.Contracts.SearchContract;
using static Youtube.Contracts.VideoDetailContract;

namespace Youtube
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public static string ChannelId { get; set; }
        public static string ChannelName { get; set; }
        public static string ChannelImg { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            IoC_Container.ServiceCollection serviceCollection = new IoC_Container.ServiceCollection();
            serviceCollection.AddSingleton<Token, Token>();
            serviceCollection.AddSingleton<YoutubeContext, YoutubeContext>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
            YoutubeAPI.Auth.Token token = ServiceProvider.GetService<Token>();

            while (!await token.IsValidScope())
            {
                MessageBox.Show("請完整授權");
                await token.ReGetTokenByCode();
            }
            YoutubeContext youtubeContext = ServiceProvider.GetService<YoutubeContext>();
            var channel = await youtubeContext.Channel.GetMyChannelAsync();
            ChannelId = channel.items[0].id;
            ChannelName = channel.items[0].snippet.title;
            ChannelImg = channel.items[0].snippet.thumbnails.medium.url;
            MainWindow mainWindow = ServiceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }

    }
}
