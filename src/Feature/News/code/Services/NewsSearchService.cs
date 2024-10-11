using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;

namespace Abbyy.Feature.News.Services
{
    // this service would go to one of the foundation projects as a general search service (probably called just 'SearchService') for the solution

    public class NewsSearchService
    {
        public SearchService()
        {
            this.SearchIndexResolver = DependencyResolver.Current.GetService<SearchIndexResolver>();
            this.SearchResultsFactory = DependencyResolver.Current.GetService<SearchResultsFactory>();
        }

        private SearchIndexResolver SearchIndexResolver { get; }

        private SearchResultsFactory SearchResultsFactory { get; }

        public IEnumerable<IQueryRoot> QueryRoots => IndexingProviderRepository.QueryRootProviders.Union(new[] {this.Settings});



        public IEnumerable<Item> GetNews(contextItem, IEnumerable<ID> templates, string facets, string date)
        {
            using (var context = ContentSearchManager.GetIndex(contextItem).CreateSearchContext())
            {
                var queryable = context.GetQueryable<SearchResultItem>();

                if (templatesemplates != null && templates.Any())
                {
                    queryable = queryable.Cast<IndexedItem>().Where(this.GetTemplatePredicates(templates));
                }

                if (!string.IsNullOrEmpty(facets))
                {
                    var facetsDict = this.ParseFacets(facets);
                    queryable = this.AddFacets(queryable, facetsDict);
                }

                var results = queryable.GetResults();

                return results.Hits.Select(h => h.Document.GetItem()).Where(item => item[Templates.NewsItem.Fields.PublishDate] == date).ToArray();
            }
        }

        private Expression<Func<IndexedItem, bool>> GetTemplatePredicates(IEnumerable<ID> templates)
        {
            var expression = PredicateBuilder.False<IndexedItem>();
            foreach (var template in templates)
            {
                expression = expression.Or(i => i.AllTemplates.Contains(IdHelper.NormalizeGuid(template)));
            }
            return expression;
        }

        private Dictionary<string, string[]> ParseFacets(string facetQueryString)
        {
            facetQueryString = HttpUtility.UrlDecode(facetQueryString);

            var returnValue = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(facetQueryString))
                return returnValue;

            var facetsValuesStrings = facetQueryString.Split(new[] { FacetsSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var facetValuesString in facetsValuesStrings)
            {
                var facetValues = facetValuesString.Split(FacetSeparator);
                if (facetValues.Length <= 1)
                    continue;

                var name = facetValues[0].ToLowerInvariant();
                var values = facetValues[1].Split(FacetValueSeparator).Distinct().ToArray();
                if (returnValue.ContainsKey(name))
                    returnValue[name] = returnValue[name].Union(values).Distinct().ToArray();
                else
                    returnValue.Add(name, values);
            }
            return returnValue;
        }

        private IQueryable<SearchResultItem> AddFacets(IQueryable<SearchResultItem> queryable, Dictionary<string, string[]> facets)
        {
            var facets = GetFacetsFromProviders();

            var addedFacetPredicate = false;
            var facetPredicate = PredicateBuilder.True<SearchResultItem>();
            foreach (var facet in facets)
            {
                if (string.IsNullOrEmpty(facet.FieldName))
                    continue;

                if (facets != null && facets.ContainsKey(facet.FieldName))
                {
                    var facetValues = facets[facet.FieldName];

                    var facetValuePredicate = PredicateBuilder.False<SearchResultItem>();
                    foreach (var facetValue in facetValues)
                    {
                        if (facetValue == null)
                            continue;
                        facetValuePredicate = facetValuePredicate.Or(item => item[facet.FieldName] == facetValue);
                    }
                    facetPredicate = facetPredicate.And(facetValuePredicate);
                    addedFacetPredicate = true;
                }
                queryable = queryable.FacetOn(item => item[facet.FieldName]);
            }
            if (addedFacetPredicate)
                queryable = queryable.Where(facetPredicate);

            return queryable;
        }

        private static IEnumerable<IQueryFacet> GetFacetsFromProviders()
        {
            // should come from a repository
        }
    }
}