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
using Youtube.Components.SearchFilterComponent;

namespace Youtube.Components.ReplyCommentComponent
{
    /// <summary>
    /// ReplyComment.xaml 的互動邏輯
    /// </summary>
    public partial class ReplyComment : UserControl
    {
        public ReplyComment()
        {
            InitializeComponent();
            DataContext = new ReplyCommentContext();
        }
        //public string CommentText
        //{
        //    get => (string)GetValue(CommandTextProperty);
        //    set => SetValue(CommandTextProperty, value);
        //}

        //public static readonly DependencyProperty CommandTextProperty =
        //    DependencyProperty.Register(
        //        nameof(CommentText),
        //        typeof(string),
        //        typeof(ReplyComment),
        //        new PropertyMetadata((d, e) =>
        //        {
        //            ReplyComment replyComment = (ReplyComment)d;
        //            ReplyCommentContext replyCommentContext = (ReplyCommentContext)replyComment.DataContext;
        //            replyCommentContext.CommentText = (string)e.NewValue;
        //        }));
        public string ParentId
        {
            get => (string)GetValue(ParentIdProperty);
            set => SetValue(ParentIdProperty, value);
        }

        public static readonly DependencyProperty ParentIdProperty =
            DependencyProperty.Register(
                nameof(ParentId),
                typeof(string),
                typeof(ReplyComment),
                new PropertyMetadata((d, e) =>
                {
                    ReplyComment replyComment = (ReplyComment)d;
                    ReplyCommentContext context = (ReplyCommentContext)replyComment.DataContext;
                    context.ParentId = (string)e.NewValue;
                }));
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(ReplyComment),
                new PropertyMetadata((d, e) =>
                {
                    ReplyComment replyComment = (ReplyComment)d;
                    ReplyCommentContext replyCommentContext = (ReplyCommentContext)replyComment.DataContext;
                    replyCommentContext.AddCommand = (ICommand)e.NewValue;
                }));

        public static readonly DependencyProperty CancelCommandProperty =
    DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(ReplyComment));

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }
    }
}
