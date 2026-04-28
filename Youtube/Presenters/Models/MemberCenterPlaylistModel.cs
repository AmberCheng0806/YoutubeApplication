using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Utility;

namespace Youtube.Presenters.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterPlaylistModel
    {
        public string PlaylistImg { get; set; }
        public string PlaylistTitle { get; set; }
        public string PlaylistDescription { get; set; }
        public string PlaylistPrivacyStatus { get; set; }
        public string PlaylistId { get; set; }
        public DateTime PublishedTime { get; set; }
        public int VideosCount { get; set; }
        public List<OptionsViewModel> Status { get; set; } = new List<OptionsViewModel>() { new OptionsViewModel("public", "公開"), new OptionsViewModel("unlisted", "不公開"), new OptionsViewModel("private", "私人") };
        public ObservableCollection<PlaylistItemVideo> PlaylistItemVideos { get; set; } = new ObservableCollection<PlaylistItemVideo>();
        public Visibility EditPlaylistModeVisibility { get; set; } = Visibility.Collapsed;
        public Visibility ExpandVideosVisibility { get; set; } = Visibility.Collapsed;
        public ICommand EditPlaylistCommand { get; set; }
        public ICommand ExpandVideosCommand { get; set; }

        public MemberCenterPlaylistModel(string playlistImg, string playlistTitle, string playlistDescription, string playlistPrivacyStatus, string playlistId, DateTime publishedTime, int videosCount, ObservableCollection<PlaylistItemVideo> videos)
        {
            PlaylistImg = playlistImg;
            PlaylistTitle = playlistTitle;
            PlaylistDescription = playlistDescription;
            PlaylistPrivacyStatus = playlistPrivacyStatus;
            PlaylistId = playlistId;
            PublishedTime = publishedTime;
            VideosCount = videosCount;
            PlaylistItemVideos = videos;
            EditPlaylistCommand = new RelayCommand(() =>
            {
                EditPlaylistModeVisibility = EditPlaylistModeVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            });
            ExpandVideosCommand = new RelayCommand(() =>
            {
                ExpandVideosVisibility = ExpandVideosVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            });
        }
    }
}
