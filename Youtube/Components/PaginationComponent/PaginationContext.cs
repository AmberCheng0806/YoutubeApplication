using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube.Presenters;
using Youtube.Utility;
using static Youtube.Contracts.PaginationContract;

namespace Youtube.Components.PaginationComponent
{
    [AddINotifyPropertyChangedInterface]
    internal class PaginationContext : IPaginationView
    {
        private IPaginationPresenter presenter;
        private Pagination Pagination { get; set; }
        public int PageIndex { get; set; } = 1;

        public int CountPerPage { get; set; } = 5;
        public int PageCount { get; set; }
        public int TotalCount
        {
            set
            {
                presenter.CreatePagesRequest(CountPerPage, value);
            }
        }

        public ObservableCollection<Page> Pages { get; set; } = new ObservableCollection<Page>();
        public ObservableCollection<int> CountPerPageOptions { get; set; } = new ObservableCollection<int>() { 5, 10, 15, 20 };

        [DependsOn(nameof(PageIndex))]
        public bool HasPreviousPage => PageIndex != 1;

        [DependsOn(nameof(PageIndex), nameof(PageCount))]
        public bool HasNextPage => PageIndex < PageCount;
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand JumpPrevPageCommand { get; }
        public ICommand JumpNextPageCommand { get; }
        public ICommand ChangePageCommand { get; }
        public ICommand ChangeCountPerPageCommand { get; }

        public PaginationContext(Pagination pagination)
        {
            presenter = new PaginationPresenter(this);
            Pagination = pagination;

            PrevPageCommand = new RelayCommand(presenter.PrevPageRequest, () => HasPreviousPage);
            NextPageCommand = new RelayCommand(presenter.NextPageRequest, () => HasNextPage);
            JumpPrevPageCommand = new RelayCommand(presenter.JumpPrevPageRequest, () => HasPreviousPage);
            JumpNextPageCommand = new RelayCommand(presenter.JumpNextPageRequest, () => HasNextPage);
            ChangePageCommand = new RelayCommand<int>(x => { presenter.ChangePageRequest(x); });
            ChangeCountPerPageCommand = new RelayCommand<int>(x =>
            {
                CountPerPage = x;
                presenter.ChangeCountPerPageRequest(x);
            });
        }

        public PaginationContext()
        {
            presenter = new PaginationPresenter(this);

            PrevPageCommand = new RelayCommand(presenter.PrevPageRequest, () => HasPreviousPage);
            NextPageCommand = new RelayCommand(presenter.NextPageRequest, () => HasNextPage);
            JumpPrevPageCommand = new RelayCommand(presenter.JumpPrevPageRequest, () => HasPreviousPage);
            JumpNextPageCommand = new RelayCommand(presenter.JumpNextPageRequest, () => HasNextPage);
            ChangePageCommand = new RelayCommand<int>(x => { presenter.ChangePageRequest(x); });
            ChangeCountPerPageCommand = new RelayCommand<int>(x =>
            {
                CountPerPage = x;
                presenter.ChangeCountPerPageRequest(x);
            });
        }

        public void RenderPages(List<Page> pages)
        {
            Pages = new ObservableCollection<Page>(pages);
        }

        public void UpdatePageIndex(int index)
        {
            PageIndex = index;
        }

        public void UpdatePageCount(int count)
        {
            PageCount = count;
        }

        public void PageIndexChanged(PaginationDTO page)
        {
            Pagination.Execute(page);
        }
    }
}
