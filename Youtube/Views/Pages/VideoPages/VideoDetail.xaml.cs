using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using Youtube.Components.VideoCardComponent;

namespace Youtube.Views.Pages.VideoPages
{
    /// <summary>
    /// VideoDetail.xaml 的互動邏輯
    /// </summary>
    public partial class VideoDetail : Page
    {
        VideoDetailContext videoDetailContext { get; set; } = new VideoDetailContext();
        public VideoDetail()
        {
            InitializeComponent();
            DataContext = videoDetailContext;
        }
    }
}
