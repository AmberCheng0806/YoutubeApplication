using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Presenters.Models;
using YoutubeAPI;
using YoutubeAPI.PlayList.Models;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterPlaylistsContext : INavigationAware
    {
        public ObservableCollection<MemberCenterPlaylistModel> Playlists { get; set; } = new ObservableCollection<MemberCenterPlaylistModel>();
        public YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        private string OriginalPlaylistTitle { get; set; }
        private string OriginalPlaylistDescription { get; set; }
        private string OriginalPlaylistPrivacy { get; set; }
        public ICommand DeletePlaylistCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteVideoCommamd { get; set; }

        public async void OnNavigatedTo(object[] parameter)
        {
            var playlist = await YoutubeContext.Playlist.GetAllAsync();
            foreach (var item in playlist.items)
            {
                var videos = await YoutubeContext.PlayListItem.GetAllAsync(item.id);
                var playlistItems = videos.items.Select(x => new PlaylistItemVideo(x.snippet.thumbnails.medium.url, x.snippet.title, x.snippet.videoOwnerChannelTitle, x.snippet.resourceId.videoId, x.id));
                Playlists.Add(new MemberCenterPlaylistModel(item.snippet.thumbnails.medium.url, item.snippet.title, item.snippet.description, item.status.privacyStatus, item.id, item.snippet.publishedAt, item.contentDetails.itemCount, new ObservableCollection<PlaylistItemVideo>(playlistItems)));
                OriginalPlaylistTitle = item.snippet.title;
                OriginalPlaylistDescription = item.snippet.description;
                OriginalPlaylistPrivacy = item.status.privacyStatus;
            }
            DeletePlaylistCommand = new RelayCommand<MemberCenterPlaylistModel>(x =>
            {
                YoutubeContext.Playlist.DeleteAsync(x.PlaylistId);
                Playlists.Remove(x);
            });
            SaveCommand = new RelayCommand<MemberCenterPlaylistModel>(async x =>
            {
                await YoutubeContext.Playlist.UpdateAsync(x.PlaylistId, x.PlaylistTitle, x.PlaylistDescription, x.PlaylistPrivacyStatus);
                x.EditPlaylistModeVisibility = Visibility.Collapsed;
            });
            CancelCommand = new RelayCommand<MemberCenterPlaylistModel>(async x =>
            {
                x.PlaylistTitle = OriginalPlaylistTitle;
                x.PlaylistDescription = OriginalPlaylistDescription;
                x.PlaylistPrivacyStatus = OriginalPlaylistPrivacy;
                x.EditPlaylistModeVisibility = Visibility.Collapsed;
            });
            DeleteVideoCommamd = new RelayCommand<PlaylistItemVideo>(async x =>
            {
                await YoutubeContext.PlayListItem.DeleteAsync(x.PlaylistVideoId);
                Playlists.FirstOrDefault(y =>
                {
                    var video = y.PlaylistItemVideos.FirstOrDefault(z => z.PlaylistVideoId == x.PlaylistVideoId);
                    if (video != null) { y.PlaylistItemVideos.Remove(video); }
                    return true;
                });
            });
        }
    }
}
