using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Youtube.Components.PlayListComponent
{
    [AddINotifyPropertyChangedInterface]
    public class PlaylistItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsAddedVideo { get; set; }
        public string PlayListItemVideoId { get; set; }
        public PlaylistItem(string id, string title, bool isAddedVideo, string playListItemVideoId)
        {
            Id = id;
            Title = title;
            IsAddedVideo = isAddedVideo;
            PlayListItemVideoId = playListItemVideoId;
        }
        public PlaylistItem() { }
    }
}
