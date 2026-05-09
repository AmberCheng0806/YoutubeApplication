using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class MemberCenterPlaylistDTO
    {
        public string PlaylistImg { get; set; }
        public string PlaylistTitle { get; set; }
        public string PlaylistDescription { get; set; }
        public string PlaylistPrivacyStatus { get; set; }
        public string PlaylistId { get; set; }
        public DateTime PublishedTime { get; set; }
        public int VideosCount { get; set; }
        public ObservableCollection<PlaylistItemVideo> PlaylistItemVideos { get; set; } = new ObservableCollection<PlaylistItemVideo>();
        public string OriginalPlaylistTitle { get; set; }
        public string OriginalPlaylistDescription { get; set; }
        public string OriginalPlaylistPrivacy { get; set; }

        public MemberCenterPlaylistDTO(string playlistImg, string playlistTitle, string playlistDescription, string playlistPrivacyStatus, string playlistId, DateTime publishedTime, int videosCount, ObservableCollection<PlaylistItemVideo> playlistItemVideos, string originalPlaylistTitle, string originalPlaylistDescription, string originalPlaylistPrivacy)
        {
            PlaylistImg = playlistImg;
            PlaylistTitle = playlistTitle;
            PlaylistDescription = playlistDescription;
            PlaylistPrivacyStatus = playlistPrivacyStatus;
            PlaylistId = playlistId;
            PublishedTime = publishedTime;
            VideosCount = videosCount;
            PlaylistItemVideos = playlistItemVideos;
            OriginalPlaylistTitle = originalPlaylistTitle;
            OriginalPlaylistDescription = originalPlaylistDescription;
            OriginalPlaylistPrivacy = originalPlaylistPrivacy;
        }
        public MemberCenterPlaylistDTO() { }
    }
}
