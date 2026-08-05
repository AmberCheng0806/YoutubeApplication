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
        public Pagination()
        {
            InitializeComponent();
            DataContext = (PaginationContext)App.ServiceProvider.GetService(typeof(PaginationContext));
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
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(Pagination),
                new PropertyMetadata((d, e) =>
                {
                    Pagination pagination = (Pagination)d;
                    PaginationContext paginationContext = (PaginationContext)pagination.DataContext;
                    paginationContext.ChangePaginationIndexCommand = (ICommand)e.NewValue;
                }));


        public static readonly DependencyProperty CommandProperty2 =
            DependencyProperty.Register(
                nameof(TotalCount),
                typeof(int),
                typeof(Pagination),
       new PropertyMetadata((d, e) =>
       {
           Pagination pagination = (Pagination)d;
           PaginationContext paginationContext = (PaginationContext)pagination.DataContext;
           paginationContext.TotalCount = (int)e.NewValue;
       }));
    }
}

