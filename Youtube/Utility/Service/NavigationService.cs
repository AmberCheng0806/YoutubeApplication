using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Youtube.Components.PaginationComponent;
using Youtube.Views.Pages;
using Page = System.Windows.Controls.Page;

namespace Youtube.Utility.Service
{
    internal class NavigationService : INavigationService
    {
        public Frame Frame { get; set; }
        public NavigationService(Frame frame) { Frame = frame; }
        public void Navigate(string pageKey, params object[] parameter)
        {
            Page currentPage = null;
            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name.Contains(pageKey) && x.BaseType == typeof(Page));
            if (type == null) throw new Exception("Page not found");
            currentPage = (Page)Activator.CreateInstance(type);
            if (currentPage.DataContext is INavigationAware aware)
            {
                aware.OnNavigatedTo(parameter);
            }
            Frame.Navigate(currentPage);
        }

    }
}
