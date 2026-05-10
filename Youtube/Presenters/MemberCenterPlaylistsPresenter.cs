using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;
using YoutubeAPI;
using static Youtube.Contracts.MemberCenterPlaylistsContract;

namespace Youtube.Presenters
{
    internal class MemberCenterPlaylistsPresenter : IMemberCenterPlaylistsPresenter
    {
        public IMemberCenterPlaylistsView MemberCenterPlaylistsView { get; set; }
        private YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        public MemberCenterPlaylistsPresenter(IMemberCenterPlaylistsView view) { MemberCenterPlaylistsView = view; }
        public async Task DeletePlaylistRequest(string playlistId)
        {
            await YoutubeContext.Playlist.DeleteAsync(playlistId);
            MemberCenterPlaylistsView.DeletePlaylist(playlistId);
        }

        public async Task GetMyPlaylistsRequest()
        {
            var playlist = await YoutubeContext.Playlist.GetAllAsync();
            var tasks = playlist.items.Select(async item =>
            {
                var videos = await YoutubeContext.PlayListItem.GetAllAsync(item.id);
                var playlistItems = videos.items.Select(x => new PlaylistItemVideo(
                    x.snippet.thumbnails.medium.url,
                    x.snippet.title,
                    x.snippet.videoOwnerChannelTitle,
                    x.snippet.resourceId.videoId,
                    x.id));
                if (item.snippet.thumbnails.medium.url == "https://i.ytimg.com/img/no_thumbnail.jpg")
                {
                    item.snippet.thumbnails.medium.url = "https://getparasol.com/wp-content/themes/parasol/images/sample.png";
                }
                return new MemberCenterPlaylistDTO(
                    item.snippet.thumbnails.medium.url,
                    item.snippet.title,
                    item.snippet.description,
                    item.status.privacyStatus,
                    item.id,
                    item.snippet.publishedAt,
                    item.contentDetails.itemCount,
                    new ObservableCollection<PlaylistItemVideo>(playlistItems),
                    item.snippet.title,
                    item.snippet.description,
                    item.status.privacyStatus);
            });
            var dtos = await Task.WhenAll(tasks);
            MemberCenterPlaylistsView.RenderPlaylists(dtos.ToList());
        }
        public async Task UpdatePlaylistRequest(string playlistId, string playlistTitle, string playlistDescription, string playlistPrivacyStatus)
        {
            await YoutubeContext.Playlist.UpdateAsync(playlistId, playlistTitle, playlistDescription, playlistPrivacyStatus);
        }

        public async Task DeleteVideoRequest(string playlistVideoId)
        {
            await YoutubeContext.PlayListItem.DeleteAsync(playlistVideoId);
            MemberCenterPlaylistsView.DeleteVideo(playlistVideoId);
        }

        public async Task CreatePlaylistRequest(string title, string status, string description)
        {
            var playlist = await YoutubeContext.Playlist.CreateAsync(title, status, description);
            if (playlist.snippet.thumbnails.medium.url == "https://i.ytimg.com/img/no_thumbnail.jpg")
            {
                playlist.snippet.thumbnails.medium.url = "https://getparasol.com/wp-content/themes/parasol/images/sample.png";
            }
            MemberCenterPlaylistsView.CreatePlaylist(new MemberCenterPlaylistDTO(playlist.snippet.thumbnails.medium.url, title, description, status, playlist.id, playlist.snippet.publishedAt, 0, new ObservableCollection<PlaylistItemVideo>(), title, description, status));
        }
    }
}
