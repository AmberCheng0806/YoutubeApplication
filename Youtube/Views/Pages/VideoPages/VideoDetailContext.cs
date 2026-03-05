using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YoutubeAPI.Video.Models;

namespace Youtube.Views.Pages.VideoPages
{
    [AddINotifyPropertyChangedInterface]
    internal class VideoDetailContext : INavigationAware
    {
        public string VideoId { get; set; }
        public void OnNavigatedTo(object[] parameter)
        {
            VideoId = parameter[0] as string;
        }
    }
}
