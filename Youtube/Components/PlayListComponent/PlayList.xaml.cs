using PropertyChanged;
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
using Youtube.Components.PaginationComponent;
using Youtube.Components.SearchFilterComponent;
using Youtube.Utility;

namespace Youtube.Components.PlayListComponent
{
    /// <summary>
    /// PlayList.xaml 的互動邏輯
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class PlayList : UserControl
    {
        public PlayList()
        {
            InitializeComponent();
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty =
         DependencyProperty.Register(
             nameof(Command),
             typeof(ICommand),
             typeof(PlayList),
             new PropertyMetadata(((d, e) =>
             {
                 PlayList playlist = (PlayList)d;
                 PlaylistItem item = (PlaylistItem)playlist.DataContext;
                 item.SaveCommand = (ICommand)e.NewValue;
             })));

        //public void Execute(PlaylistItem condition)
        //{
        //    if (Command?.CanExecute(condition) == true)
        //    {
        //        Command.Execute(condition);
        //    }
        //}
    }
}
