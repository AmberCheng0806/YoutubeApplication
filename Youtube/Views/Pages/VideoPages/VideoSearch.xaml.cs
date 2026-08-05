using IoC_Container.Attributes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube.Utility.Service;

namespace Youtube.Views.Pages.VideoPages
{
    /// <summary>
    /// VideoSearch.xaml 的互動邏輯
    /// </summary>
    [Singleton]
    public partial class VideoSearch : Page
    {
        public VideoSearch(VideoSearchContext videoSearchContext)
        {
            InitializeComponent();
            DataContext = videoSearchContext;
        }
    }
}
