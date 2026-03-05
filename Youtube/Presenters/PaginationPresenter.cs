using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Components.PaginationComponent;
using static Youtube.Contracts.PaginationContract;

namespace Youtube.Presenters
{
    internal class PaginationPresenter : IPaginationPresenter
    {
        IPaginationView PaginationView { get; set; }
        public int TotalCount { get; set; } = 173;
        List<Page> Pages { get; set; } = new List<Page>();
        public int PageIndex { get; set; } = 1;
        public int DisplayPageNumber { get; set; } = 10;
        public int PageCount { get; set; }
        public int CountPerPage { get; set; } = 5;
        public PaginationPresenter(IPaginationView paginationView)
        {
            this.PaginationView = paginationView;
        }

        public void CreatePagesRequest(int CountPerPage, int TotalCount)
        {
            this.CountPerPage = CountPerPage;
            this.TotalCount = TotalCount;
            CreatePages();
            PaginationView.RenderPages(Pages);
        }

        private void CreatePages()
        {
            Pages.Clear();
            int page = (int)Math.Ceiling((double)(TotalCount / (CountPerPage * 1.0)));
            PageCount = page;
            PaginationView.UpdatePageCount(PageCount);
            PageIndex = Math.Min(PageCount, PageIndex);
            PaginationView.UpdatePageIndex(PageIndex);
            int start = (int)(Math.Floor((double)((PageIndex - 1) / DisplayPageNumber)) * DisplayPageNumber + 1);
            int end = start + DisplayPageNumber - 1;
            end = end >= page ? page : end;
            for (int i = start; i <= end; i++)
            {
                Pages.Add(new Page(i, false));
            }
            Pages[PageIndex - start].IsActived = true;
        }

        public void ChangePageRequest(int page)
        {
            PageIndex = page;
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }

        public void PrevPageRequest()
        {
            PageIndex--;
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }

        public void NextPageRequest()
        {
            PageIndex++;
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }

        public void JumpPrevPageRequest()
        {
            PageIndex = Math.Max(1, PageIndex - 10);
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }

        public void JumpNextPageRequest()
        {
            PageIndex = Math.Min(PageCount, PageIndex + 10);
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }

        public void ChangeCountPerPageRequest(int count)
        {
            CountPerPage = count;
            CreatePages();
            PaginationView.RenderPages(Pages);
            PaginationView.PageIndexChanged(new PaginationDTO(PageIndex, CountPerPage));
        }
    }
}
