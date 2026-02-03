using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Components.PaginationComponent
{
    [AddINotifyPropertyChangedInterface]
    public class Page
    {
        public int Number { get; set; }
        public bool IsActived { get; set; }

        public Page(int number, bool isActived)
        {
            Number = number;
            IsActived = isActived;
        }
    }
}
