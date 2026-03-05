using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Components.PaginationComponent
{
    public class PaginationDTO
    {
        public int PaginationIndex { get; set; }
        public int CountPerPage { get; set; }

        public PaginationDTO(int paginationIndex, int countPerPage)
        {
            PaginationIndex = paginationIndex;
            CountPerPage = countPerPage;
        }

    }
}
