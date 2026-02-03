using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    [AddINotifyPropertyChangedInterface]
    public class OptionsViewModel
    {
        public string Key { get; }
        public string DisplayName { get; }

        public bool IsChecked { get; set; }

        public OptionsViewModel(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
