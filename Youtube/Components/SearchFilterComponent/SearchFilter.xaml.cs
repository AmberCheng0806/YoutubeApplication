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
using Youtube.Views;

namespace Youtube.Components.SearchFilterComponent
{
    /// <summary>
    /// SearchFilter.xaml 的互動邏輯
    /// </summary>
    public partial class SearchFilter : UserControl
    {
        public SearchFilter()
        {
            InitializeComponent();
            DataContext = new SearchFilterContext(this);
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
                typeof(SearchFilter),
                new PropertyMetadata(null));

        public void Execute(SearchFilterDTO condition)
        {
            if (Command?.CanExecute(condition) == true)
            {
                Command.Execute(condition);
                Window window = this.Parent as Window;
                window?.Close();
            }
        }
    }
}
