using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube.Components.ReplyCommentComponent;

namespace Youtube.Components.CommentComponent
{
    /// <summary>
    /// Comment.xaml 的互動邏輯
    /// </summary>
    public partial class Comment : UserControl
    {
        public Comment()
        {
            InitializeComponent();
        }
        public ICommand ShowReplyCommand
        {
            get => (ICommand)GetValue(ShowReplyCommandProperty);
            set => SetValue(ShowReplyCommandProperty, value);
        }

        public static readonly DependencyProperty ShowReplyCommandProperty =
            DependencyProperty.Register(
                nameof(ShowReplyCommand),
                typeof(ICommand),
                typeof(Comment),
                new PropertyMetadata((d, e) =>
                {
                    Comment comment = (Comment)d;
                    CommentItem commentItem = (CommentItem)comment.DataContext;
                    commentItem.CancelReplyCommentsCommand = (ICommand)e.NewValue;
                }));
    }
}
