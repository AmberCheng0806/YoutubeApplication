using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Youtube.Utility.Service
{
    public interface INavigationService
    {
        void Navigate(string pageKey, params object[] parameter);
    }
}
