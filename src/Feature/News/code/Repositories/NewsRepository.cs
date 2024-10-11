using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Abbyy.Feature.News.Models;

namespace Abbyy.Feature.News.Repositories
{
    [Service(typeof(INewsRepository))]
    public class NewsRepository : INewsRepository
    {
        private readonly NewsSearchService newsSearchService;

        public NewsRepository()
        {
            this.newsSearchService = new NewsSearchService();
        }

        public IEnumerable<Item> GetNews(Item contextItem, string facets, string date)
        {
            var results = newsSearchService.GetNews(contextItem, new[] { Templates.NewsItem.ID }, facets, date);
            return results.OrderByDescending(i => i[Templates.NewsItem.Fields.PublishDate]);
        }

        public IEnumerable<NewsFacet> GetNewsFacets(Item contextItem)
        {
            throw new NotImplementedException();
        }
    }
}