using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube.Utility;

namespace Youtube.Components.PaginationComponent
{
    [AddINotifyPropertyChangedInterface]
    internal class PaginationContext
    {
        public int TotalCount { get; set; } = 173;
        public int PageCount { get; set; }
        public int PageIndex { get; set; } = 1;
        public int DisplayPageNumber { get; set; } = 10;
        public int CountPerPage { get; set; } = 5;
        public ObservableCollection<Page> Pages { get; set; } = new ObservableCollection<Page>();
        public ObservableCollection<int> CountPerPageOptions { get; set; } = new ObservableCollection<int>() { 5, 10, 15, 20 };

        [DependsOn(nameof(PageIndex))]
        public bool HasPreviousPage => PageIndex != 1;

        [DependsOn(nameof(PageIndex), nameof(PageCount))]
        public bool HasNextPage => PageIndex < PageCount;
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        [DependsOn(nameof(PageIndex))]
        public bool CanRenderPages => (StartPage > PageIndex || PageIndex > EndPage);
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand JumpPrevPageCommand { get; }
        public ICommand JumpNextPageCommand { get; }
        public ICommand ChangePageCommand { get; }
        public ICommand ChangeCountPerPageCommand { get; }

        public PaginationContext()
        {
            CreatePages();

            PrevPageCommand = new RelayCommand(() =>
            {
                if (HasPreviousPage) PageIndex--;
                ResetPageIsActive();
                if (CanRenderPages) CreatePages();
            });
            NextPageCommand = new RelayCommand(() =>
            {
                if (HasNextPage) PageIndex++;
                ResetPageIsActive();
                if (CanRenderPages) CreatePages();
            });
            JumpPrevPageCommand = new RelayCommand(() =>
            {
                if (HasPreviousPage) PageIndex = Math.Max(1, PageIndex - 10);
                ResetPageIsActive();
                if (CanRenderPages) CreatePages();
            });
            JumpNextPageCommand = new RelayCommand(() =>
            {
                if (HasNextPage) PageIndex = Math.Min(PageCount, PageIndex + 10);
                ResetPageIsActive();
                if (CanRenderPages) CreatePages();
            });
            ChangePageCommand = new RelayCommand<int>(x => { PageIndex = x; ResetPageIsActive(); });
            ChangeCountPerPageCommand = new RelayCommand(() => CreatePages());
        }
        private void CreatePages()
        {
            Pages.Clear();
            int page = (int)Math.Ceiling((double)(TotalCount / (CountPerPage * 1.0)));
            PageCount = page;
            PageIndex = Math.Min(PageCount, PageIndex);
            int start = (int)(Math.Floor((double)((PageIndex - 1) / DisplayPageNumber)) * DisplayPageNumber + 1);
            int end = start + DisplayPageNumber - 1;
            end = end >= page ? page : end;
            StartPage = start;
            EndPage = end;
            for (int i = start; i <= end; i++)
            {
                Pages.Add(new Page(i, false));
            }
            Pages[PageIndex - start].IsActived = true;
        }
        private void ResetPageIsActive()
        {
            foreach (Page page in Pages)
            {
                page.IsActived = page.Number == PageIndex;
            }
        }
    }
}
