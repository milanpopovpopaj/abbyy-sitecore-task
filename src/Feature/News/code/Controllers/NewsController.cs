using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abbyy.Feature.News.Repositories;

namespace Abbyy.Feature.News.Controllers
{
    public class NewsController : Controller
    {
        public NewsController(INewsRepository newsRepository)
        {
            this.Repository = newsRepository;
        }

        private INewsRepository Repository { get; }

        // we could add paging here
        public ActionResult NewsListing(string facets, string date)
        {
            var items = this.Repository.GetNews(RenderingContext.Current.Rendering.Item, facets, date);
            return this.View("NewsListing", items);
        }

        public ActionResult NewsFacets()
        {
            var facets = this.Repository.GetNewsFacets(RenderingContext.Current.Rendering.Item);
            return this.View("NewsFacets", facets);
        }
    }
}