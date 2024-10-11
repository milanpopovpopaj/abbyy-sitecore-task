using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abbyy.Feature.News
{
    public static class Templates
    {
        public static class NewsItem
        {
            public static readonly ID Id = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
                public static readonly ID PublishDate = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
                public static readonly ID Category = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
                public static readonly ID Description = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
            }
        }

        public static class NewsFolder
        {
            public static readonly ID ID = new ID("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}");
        }
    }
}