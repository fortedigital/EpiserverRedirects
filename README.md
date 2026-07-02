# Redirects module for EPiServer

The module has the possibility to automatically manage redirects in the solution. Key features:
- Allow editors to manage redirects from old URLs, add redirects from some legacy addresses, etc.,
   - This is possible via the user interface (adding new "Redirects" tab in the EPiServer panel top menu)
- Import redirects from file - see section `Import redirects from CSV file`
- Create redirects automatically.
   - There is an automatic redirect created every time the path to a published page changes, taking into account different scenarios:
      - changing the URL Segment of a page,
      - changing the URL segment of any ancestor (either published or not),
      - changing the parent of the page, moving the page in content structure,
      - moving any ancestor of the page in content structure,
      - restoring from trash to a different location than the original, or when the URL of this location changed.



Basic installation scenario
------------
1. The package can be found in the official NuGet repository.
   ```Install-Package Forte.EpiserverRedirects```

2. Register services in your Startup class and provide an optional configure action:

```c#
public void ConfigureServices(IServiceCollection services)
{
    // (...)
    services.ConfigureEpiserverRedirects(
        options =>
        {
            // configure caching, priority, etc
            options.Caching.UrlRedirectCacheEnabled = true;
            options.PreserveQueryString = false;
            options.AddAutomaticRedirects = false;
            options.SystemRedirectRulePriority = 80;
            options.DefaultRedirectRulePriority = 100;
        }
        repoConfig =>
        {
            // configure custom rule store here. Skip to fallback to Dynamic Data Store.
        });
    // (...)
}
```

3. Configure the application in your Startup class:

```c#
public void Configure(IApplicationBuilder app)
{
    // (...)
    app.UseEpiserverRedirects();
    // (...)
}
```

Custom Rule Store
-------------
By default, redirect rules are stored in Dynamic Data Store. You can implement custom store using the `IRedirectRuleRepository` interface.
Refer to the `Forte.EpiserverRedirects.SqlServer` package for SQL Server implementation or a more generic `Forte.EpiserverRedirects.EntityFramework` package that can be helpful in order to implement custom, EF-based repositories.

Cache
-------------

The default EPiServer mechanism is used to manage cache and enable it to work in a distributed environment. In this package we leverage this mechanism to cache redirect response for a given URL.

Please keep in mind that caching is disabled by default but you can change this through the configuration.

Automatic redirects
------------

Creating automatic redirects on various content events is enabled by default. You can disable it globally through the configuration.

There is also a possibility to temporarily disable this functionality by using `DisabledAutomaticRedirectsScope`:

```c#
using (new DisabledAutomaticRedirectsScope())
{
    // (...)
}
```

Manage Redirections
------------
There are two ways to manage redirections:
1. **Using user interface**
2. **By injecting ```IRedirectRuleRepository``` to your services**

Import redirects from CSV file
-------------

To import redirects, in the "Redirects" tab upload a CSV file in the following format (no columns headers, semicolon as the delimiter)

Columns (in order):

| # | Column | Required | Description |
|---|--------|----------|-------------|
| 0 | old URL | yes | Old URL path, without the domain |
| 1 | new URL | yes | New URL path, without the domain |
| 2 | content id | no | Id of the content to redirect to; leave empty to redirect to the new URL path |
| 3 | rule type | yes | `ExactMatch` or `Regex` |
| 4 | redirect type | yes | `Permanent` or `Temporary` |
| 5 | priority | no | Integer priority (lower number wins); leave empty to use the default |
| 6 | is active | yes | `TRUE` or `FALSE` |
| 7 | notes | yes | Free-text comment |
| 8 | host | no | Host the rule applies to; leave empty to apply to all hosts |
| 9 | content provider name | yes | Key of the content provider used to resolve the content id |

Example:
```
/easee;/elbillader/;;ExactMatch;Permanent;1;TRUE;COMMENT;;
/fondet;/los-fondet/;42;ExactMatch;Permanent;1;TRUE;COMMENT;example.com;CommerceProvider
```
When uploading files multiple times, duplicate redirections are overwritten.
