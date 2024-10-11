using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Abbyy.Feature.News.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<Item> GetNews(Item contextItem, string facets, string date);
        IEnumerable<NewsFacet> GetNewsFacets(Item contextItem)
    }
}