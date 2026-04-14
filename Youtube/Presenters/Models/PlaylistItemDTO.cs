using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class PlaylistItemDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsAddedVideo { get; set; }
        public string PlayListItemVideoId { get; set; }

        public PlaylistItemDTO(string id, string title, bool isAddedVideo, string playListItemVideoId)
        {
            Id = id;
            Title = title;
            IsAddedVideo = isAddedVideo;
            PlayListItemVideoId = playListItemVideoId;
        }
        public PlaylistItemDTO() { }
    }
}
