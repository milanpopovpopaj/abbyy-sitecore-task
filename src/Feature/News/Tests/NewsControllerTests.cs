using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.FakeDb.AutoFixture;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Abbyy.Feature.News.Controllers;
using Abbyy.Feature.News.Repositories;

namespace Abbyy.Feature.News.Tests
{
    public class NewsControllerTests
    {
        [Theory]
        public void NewsListing_ShouldReturnViewResult(Db db)
        {
            var itemId = ID.NewID;
            using (Db db = new Db()
                            {
                                db.Add(new DbItem("Folder Name", itemId, Templates.NewsFolder.ID));
                            })
            {
                // Arrange
                var repository = new NewsRepository();
                var controller = new NewsController(repository);


                var contextItem = db.GetItem(itemId);
                var context = new RenderingContext();

                context.Rendering = new Rendering();
                context.Rendering.Item = contextItem;

                    ContextService.Get().Push(context);

                // Act
                var list = controller.NewsListing("facets...", "March 22, 2020");

                // Assert      
                list.Should().BeOfType<ViewResult>();
            }
        }

        [Theory]
        public void NewsListing_ShouldBeEmpty(Db db)
        {
            var itemId = ID.NewID;
            using (Db db = new Db()
                            {
                                db.Add(new DbItem("Folder Name", itemId, Templates.NewsFolder.ID));
                            })
            {
                // Arrange
                var repository = new NewsRepository();
                var controller = new NewsController(repository);

                var contextItem = db.GetItem(itemId);
                var context = new RenderingContext();

                context.Rendering = new Rendering();
                context.Rendering.Item = contextItem;

                ContextService.Get().Push(context);

                // Act
                var list = controller.NewsListing("facets...", "March 22, 2020");

                // Assert      
                list.Should().BeEmpty();
            }
        }

        [Theory]
        public void NewsListing_ShouldReturnNews(Db db)
        {
            var itemId = ID.NewID;
            using (Db db = new Db()
                            {
                                db.Add(new DbItem("Folder Name", itemId, Templates.NewsFolder.ID))
                                {
                                    new DbItem("News Item 1", ID.NewID, Templates.NewsItem.ID),
                                    new DbItem("News Item 2", ID.NewID, Templates.NewsItem.ID)
                                }
                            })
            {
                // Arrange
                var repository = new NewsRepository();
                var controller = new NewsController(repository);

                var contextItem = db.GetItem(itemId);
                var context = new RenderingContext();

                context.Rendering = new Rendering();
                context.Rendering.Item = contextItem;

                ContextService.Get().Push(context);

                // Act
                var list = controller.NewsListing("facets...", "March 22, 2020");

                // Assert      
                list.Should().HaveCount(2);
            }
        }
    }
}
