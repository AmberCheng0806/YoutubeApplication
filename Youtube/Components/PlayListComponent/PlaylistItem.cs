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
        public bool IsAddedPlaylist { get; set; }
        public PlaylistItem(string id, string title, bool isAddedPlaylist)
        {
            Id = id;
            Title = title;
            IsAddedPlaylist = isAddedPlaylist;
        }
        public ICommand SaveCommand { get; set; }
    }
}
