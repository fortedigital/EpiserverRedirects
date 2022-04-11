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


2. Register services in your Startup class:

   a) by providing `Microsoft.Extensions.Configuration.IConfiguration` object:

    ```c#
    services.AddEpiserverRedirects(_configuration);
    ```

   b) by providing configure action:

    ```c#
    services.AddEpiserverRedirects(options => ...);
    ```

3. Configure the application in your Startup class:

```c#
app.UseEpiserverRedirects();
```

Configuration
-------------

When using the first method of registering services, you can change options using your configuration provider.

An example for `appsettings.json`:

 ```json
 {
   "Forte.EpiserverRedirects": {
     "Caching": {
       "UrlRedirectCacheEnabled": false,
       "AllRedirectsCacheEnabled": false
     },
     "PreserveQueryString": false,
     "AddAutomaticRedirects": true,
     "SystemRedirectRulePriority": 100,
     "DefaultRedirectRulePriority": 100
   }
 }
 ```


Cache
-------------

The default EPiServer mechanism is used to manage cache and enable it to work in a distributed environment. There are two levels of cache:
1. **Cache all redirect entries**
2. **Cache redirect response for given URL**

Caching is disabled by default. You can change this through the configuration (see the previous section).

Manage Redirections
------------
There are two ways to manage redirections:
1. **Using user interface**
2. **By injecting ```IRedirectRuleRepository``` to your services**

Import redirects from CSV file
-------------

To import redirects, in the "Redirects" tab upload a CSV file in the following format (no columns headers, semicolon as the delimiter)

Columns:
- old URL (just URL paths, without the domains)
- new URL (just URL paths, without the domains)
- type (Permanent or Temporary)
- rule type (ExactMatch or Regex)
- is active (TRUE or FALSE)
- comment
- priority (int)
- match to content (TRUE or FALSE) TRUE enforces that redirect is to URL, not to the ContentReference

Example:
```
/easee;/elbillader/;Permanent;ExactMatch;TRUE;COMMENT;1;FALSE  
/fondet;/los-fondet/;Permanent;ExactMatch;TRUE;COMMENT;1;FALSE
```
When uploading files multiple times, duplicate redirections are overwritten.

