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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube.Components.SearchFilterComponent;
using Youtube.Components.VideoCardComponent;
using Youtube.Presenters.Models;
using Youtube.Utility;
using Youtube.Utility.Service;
using Youtube.Views.Pages.VideoPages;
using YoutubeAPI;
using YoutubeAPI.Video;
using NavigationService = Youtube.Utility.Service.NavigationService;
namespace Youtube.Views
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowContext MainWindowView { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            App.NavigationService = new NavigationService(PageContainer);
            MainWindowView = new MainWindowContext(App.NavigationService);
            DataContext = MainWindowView;
        }
    }
}
