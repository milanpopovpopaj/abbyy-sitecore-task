using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abbyy.Feature.News.Models
{
    public class NewsFacet
    {
        public string Title { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}