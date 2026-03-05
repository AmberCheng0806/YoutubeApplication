using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;

namespace Youtube.Contracts
{
    internal class SearchContract
    {
        internal interface ISearchView
        {
            void SearchResponse(List<VideoCardDTO> respnose);
        }

        internal interface ISearchPresenter
        {
            Task SearchRequest(SearchRequestDTO searchRequestDTO);
        }
    }
}
