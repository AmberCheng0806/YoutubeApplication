using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Youtube.Components.ReplyCommentComponent
{
    [AddINotifyPropertyChangedInterface]
    internal class ReplyCommentContext
    {
        public string ChannelImg => App.ChannelImg;
        public string CommentText { get; set; }
        public string ParentId { get; set; }
        public ICommand AddCommand { get; set; }
    }
}
