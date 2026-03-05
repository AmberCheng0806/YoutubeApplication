using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;
using YoutubeAPI;
using static Youtube.Contracts.SearchContract;
using static YoutubeAPI.Search.Models.Search;

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
        public async Task SearchRequest(SearchRequestDTO searchRequestDTO)
        {
            int videoCategoryId = (int)searchRequestDTO.videoCategoryId;
            var result = await Context.Search.GetAllAsync(keyword: searchRequestDTO.keyword, type: searchRequestDTO.type, publishedAfter: searchRequestDTO.publishedAfter,
             videoDuration: searchRequestDTO.videoDuration, videoCategoryId: videoCategoryId);

            string ids = string.Join(",", result.items.Select(x => x.id.videoId).ToArray());
            var videoStatistic = await Context.Video.GetByVideoIdAsync(ids);
            List<VideoCardDTO> videoCardDTOs = new List<VideoCardDTO>();
            int count = 0;
            foreach (var resultItem in result.items)
            {
                string title = resultItem.snippet.title;
                string channelTitle = resultItem.snippet.channelTitle;
                DateTime publishedAt = resultItem.snippet.publishedAt;
                string url = resultItem.snippet.thumbnails.medium.url;
                string id = resultItem.id.videoId;
                string viewCount = videoStatistic.items[count].statistics.viewCount;
                VideoCardDTO videoCardDTO = new VideoCardDTO(title, id, url, channelTitle, publishedAt, viewCount);
                videoCardDTOs.Add(videoCardDTO);
                count++;
            }
            //List<VideoCardDTO> videoCardDTOs = new List<VideoCardDTO>();
            //foreach (var resultItem in result.items)
            //{
            //    string title = resultItem.snippet.title;
            //    string channelTitle = resultItem.snippet.channelTitle;
            //    DateTime publishedAt = resultItem.snippet.publishedAt;
            //    string url = resultItem.snippet.thumbnails.medium.url;
            //    string id = resultItem.id.videoId;
            //    var videoStatistic = await Context.Video.GetByVideoIdAsync(id);
            //    string viewCount = videoStatistic.items[0].statistics.viewCount;
            //    VideoCardDTO videoCardDTO = new VideoCardDTO(title, url, channelTitle, publishedAt, viewCount);
            //    videoCardDTOs.Add(videoCardDTO);
            //}
            View.SearchResponse(videoCardDTOs);
        }
    }
}
