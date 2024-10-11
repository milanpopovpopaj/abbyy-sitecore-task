# Developing a Sitecore Component

## Goal
Develop a Sitecore component that will display a list of news items with the ability to filter by date and category.

## Requirements

1. Creating Templates:
	- Create templates for news and news categories in Sitecore.
	- Example News template:
		- Title (Single-Line Text)
		- Publish Date (Datetime)
		- Category (Droplist, linked to the category template)
		- Description (Rich Text)
 
2. Creating Content:
    - Create several sample news items and categories in the Sitecore content tree.
 
3. Component Development:
	- Create an MVC component to display the list of news items.
	- Implement the ability to filter news by date and category.
	- Use Sitecore's Search API to implement filtering.
 
4. Data Display:
	- Create a view (Razor) to display the list of news items.
	- Provide a UI for selecting category and date for filtering.
 
5. Testing:
	- Write tests for the component to ensure proper functionality of filtering and news display.
	- Test the component in various usage scenarios (e.g., no news items, multiple categories, etc.).

# Implementation and explanations:

To be able to display a list of news items in Sitecore with the ability to filter by date and category, I would perform the following steps (and many more probably):

> Let's assume that we already have the layout.

1. Create News Listing page type/template. Also add standard values for the template to configure presentation details, default values, tokens etc...

2. Create a news template with required fields: **Title**, **Publish Date**, **Category**, **Description**).
	- For the Publish Date field:
		- Make it shared because it should be the same for all languages and all numbered versions.
	- For the news category field:
		- Create a template for news category.
		- Create a list of category items under some shared folder in Sitecore content tree, and then set the Source of the Category filed of the News template to point to that shared folder. In that case Content Authors would be able to chose the news category when creating news datasources.

3. Create sample news items in the Sitecore content tree. If the number of items gets big, we can concider using a bucket.

4. Create rendering definition item for the News Item. That would probably be a View Rendering, so set its Path, Datasource Location, Compatible Renderings... fields. Maybe create a custom rendering parameters as well.

5. Create rendering definition item for the News listing. That would be a Controller Rendering, so set its Controller, Controller Action... fields.

6. Add news listing rendering to the appropriate placeholder settings item (or create it if needed).

7. For the filtering by category I would use search facets (like it is partialy implemented in the sample solution). When a facet is selected (or unselected), the query string would be updated and the page would be reloaded. That's why NewsController's NewsListing action accepts the facets parameter.

8. So we should create a rendering definition item for the News Facets component as well.

9. For the filtering by date, I would probably add some kind of date picker to the news listing page.


## Regarding the code/solution:
I've decided to use a Helix architecture. I've added a simple News feature. It has the MVC project, as well as a unit tests project. Of course, in the real world, there would also be a serialization project with serialized Sitecore news items.

In the solution you can find the following files:
- **Templates.cs** that holds necessary IDs
- **NewsController** with NewsListing and NewsFacets actions which get necessary data from repository and pass it to the views
- **NewsRepository** which utilizes NewsSearchService to get data from the search index
- **NewsSearchService** which is used to get news items based on template, facets and date (in real world this service would be located in one of the foundation projects as a general search service (probably called just **SearchService**) for the solution)
- Razor views for **NewsListing** and **NewsFacets**.
- **NewsControllerTests** with a few unit tests
