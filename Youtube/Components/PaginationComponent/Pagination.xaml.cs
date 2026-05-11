using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Youtube.Utility;

namespace Youtube.Components.PaginationComponent
{
    /// <summary>
    /// Pagination.xaml 的互動邏輯
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class Pagination : UserControl
    {
        PaginationContext PaginationContext { get; set; }
        public Pagination()
        {
            InitializeComponent();
            PaginationContext = new PaginationContext(this);
            DataContext = PaginationContext;
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public int TotalCount
        {
            get => (int)GetValue(CommandProperty2);
            set
            {
                SetValue(CommandProperty2, value);
                //PaginationContext.TotalCount = value;
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(Pagination),
                new PropertyMetadata(null));


        public static readonly DependencyProperty CommandProperty2 =
            DependencyProperty.Register(
                nameof(TotalCount),
                typeof(int),
                typeof(Pagination),
       new PropertyMetadata(null));

        public void Execute(PaginationDTO condition)
        {
            Command.Execute(condition);
        }
    }
}

