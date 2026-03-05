using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Views.Pages
{
    internal interface INavigationAware
    {
        void OnNavigatedTo(object[] parameter);
    }
}
