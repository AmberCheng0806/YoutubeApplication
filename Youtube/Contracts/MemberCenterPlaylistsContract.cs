using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;

namespace Youtube.Contracts
{
    internal class MemberCenterPlaylistsContract
    {
        public interface IMemberCenterPlaylistsView
        {
            void RenderPlaylists(List<MemberCenterPlaylistDTO> memberCenterPlaylistDTOs);
            void DeletePlaylist(string playlistId);
            void DeleteVideo(string playlistVideoId);
            void CreatePlaylist(MemberCenterPlaylistDTO memberCenterPlaylistDTO);
        }

        public interface IMemberCenterPlaylistsPresenter
        {
            Task GetMyPlaylistsRequest();
            Task DeletePlaylistRequest(string playlistId);
            Task UpdatePlaylistRequest(string playlistId, string playlistTitle, string playlistDescription, string playlistPrivacyStatus);
            Task DeleteVideoRequest(string playlistVideoId);
            Task CreatePlaylistRequest(string title, string status, string description);
        }
    }
}
