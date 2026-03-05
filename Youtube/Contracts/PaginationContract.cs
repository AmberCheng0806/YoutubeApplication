using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Components.PaginationComponent;

namespace Youtube.Contracts
{
    internal class PaginationContract
    {
        public interface IPaginationView
        {
            void RenderPages(List<Page> pages);
            void UpdatePageIndex(int index);
            void UpdatePageCount(int count);
            void PageIndexChanged(PaginationDTO page);

        };
        public interface IPaginationPresenter
        {
            void CreatePagesRequest(int CountPerPage, int TotalPages);
            void ChangePageRequest(int page);
            void PrevPageRequest();
            void NextPageRequest();
            void JumpPrevPageRequest();
            void JumpNextPageRequest();
            void ChangeCountPerPageRequest(int count);
        };
    }
}
