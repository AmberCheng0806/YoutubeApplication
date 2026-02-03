using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;
using YoutubeAPI;
using static Youtube.Contracts.SearchContract;

namespace Youtube.Presenters
{
    internal class SearchPresenter : ISearchPresenter
    {
        public ISearchView View { get; set; }
        public YoutubeContext Context { get; set; } = new YoutubeContext();
        public SearchPresenter(ISearchView view)
        {
            this.View = view;
        }
        public async void SearchRequest(SearchRequestDTO searchRequestDTO)
        {
            int videoCategoryId = (int)searchRequestDTO.videoCategoryId;
            var result = await Context.Search.GetAllAsync(keyword: searchRequestDTO.keyword, type: searchRequestDTO.type, publishedAfter: searchRequestDTO.publishedAfter,
             videoDuration: searchRequestDTO.videoDuration, videoCategoryId: videoCategoryId);
            List<VideoCardDTO> videoCardDTOs = new List<VideoCardDTO>();
            foreach (var resultItem in result.items)
            {
                string title = resultItem.snippet.title;
                string channelTitle = resultItem.snippet.channelTitle;
                DateTime publishedAt = resultItem.snippet.publishedAt;
                string url = resultItem.snippet.thumbnails.medium.url;
                string id = resultItem.id.videoId;
                var videoStatistic = await Context.Video.GetByVideoIdAsync(id);
                string viewCount = videoStatistic.items[0].statistics.viewCount;
                VideoCardDTO videoCardDTO = new VideoCardDTO(title, url, channelTitle, publishedAt, viewCount);
                videoCardDTOs.Add(videoCardDTO);
            }
            View.SearchResponse(videoCardDTOs);
        }
    }
}
