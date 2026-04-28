using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube.Presenters.Models;
using YoutubeAPI;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterVideosContext : INavigationAware
    {
        public ObservableCollection<MemberCenterVideoModel> Videos { get; set; } = new ObservableCollection<MemberCenterVideoModel>();
        public YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        public async void OnNavigatedTo(object[] parameter)
        {
            var search = await YoutubeContext.Search.GetMyVideos();
            string videoString = string.Join(",", search.items.Select(x => x.id.videoId).ToArray());
            var videoDetail = await YoutubeContext.Video.GetByVideoIdAsync(videoString);
            foreach (var item in videoDetail.items)
            {
                Videos.Add(new MemberCenterVideoModel(item.snippet.thumbnails.medium.url, item.snippet.title, item.snippet.description, item.status.privacyStatus, item.snippet.publishedAt, item.statistics.viewCount, item.statistics.commentCount, item.statistics.likeCount, item.statistics.dislikeCount));
            }
        }
    }
}
