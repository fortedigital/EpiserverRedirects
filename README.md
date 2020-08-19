# EPiAutoAlias for EPiServer

The module has the possibility to automatically manage redirects in solution. Key features:
 - Allow editors to manage redirects from old urls, add redirects from some legacy addresses etc.,
    - This is possible via user interface (adding new "Redirects" tab in Episerver panel top menu)
 - Import redirects from file - see section `Import redirects from CSV file`
 - Create redirects automatically. 
    - There is automatic redirect created every time the path to a published page changes, taking into account different scenarios:
        - changing URL Segment of a page,
        - changing URL segment of any ancestor (either published or not),
        - changing parent of the page, moving the page in content structure,
        - moving any ancestor of the page in content structure,
        - restoring from trash to a different location than original, or when URL of this location changed.



Basic installation scenario
------------
1. The package can be found in official NuGet repository.
```Install-Package Forte.EpiserverRedirects``` 


2. Configure module in web.config

```xml
  <episerver.shell>
    <protectedModules rootPath="~/EPiServer/">
      <add name="Forte.EpiserverRedirects">
        <assemblies>
          <add assembly="Forte.EpiserverRedirects" />
        </assemblies>
      </add>
    </protectedModules>
  </episerver.shell>
```


3. For version 1.x.x, add ```UrlRewriteMiddleware``` into Owin Pipeline in ```Startup``` class:

```c#
app.Use(typeof(UrlRewriteMiddleware));
```


For version 2.x.x you can use both OwinMiddleware and HttpModule.

a) For OwinMiddleware:
  Add ```RedirectMiddleware``` into Owin Pipeline in ```Startup``` class

  ```c#
    app.Use(typeof(RedirectMiddleware));
  ```

b) For HttpModule:
  Add ```HttpModule``` in web.config file

  ```xml
   <configuration> 
   <system.webServer> 
    <modules> 
     <add name="Redirects" type="Forte.EpiserverRedirects.AspNet.HttpModule, Forte.EpiserverRedirects" />
    </modules> 
   </system.webServer> 
  </configuration>
  ```

Manage Redirections
------------
There are two ways to manage redirections:
1. **Using user interface**
2. **Using public interface**

a) For version 1.x.x, using ```IUrlRedirectsService```:

```       c#
          public interface IUrlRedirectsService
          {
              IQueryable<UrlRewriteModel> GetAll();
              UrlRedirectsDto Post(UrlRedirectsDto urlRedirectsDto);
              UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto);
              void Delete(Guid id);
          }
```
b) For version 2.x.x, using ```IRedirectRuleRepository```:
    
```     c#
        public interface IRedirectRuleRepository : IQueryable<RedirectRule>
        {
            RedirectRule GetById(Guid id);
            RedirectRule Add(RedirectRule redirectRule);
            RedirectRule Update(RedirectRule redirectRule);
            bool Delete(Guid id);
        }
```
    
Configuration
-------------

There is possibility to disable adding redirects to moved content automatically. To do this, set

For version 1.x.x:
 `Forte.EpiserverRedirects.UrlRewritePlugin.Configuration.AddAutomaticRedirects` flag to `false`.

For version 2.x.x:
 `Forte.EpiserverRedirects.Configuration.AddAutomaticRedirects` flag to `false`.


Import redirects from CSV file
-------------

To import redirects, in "Redirects" tab upload a CSV file containing columns:
- old URL (just URL paths, without the domains)
- new URL (just URL paths, without the domains)
- type (Pernament or Temporary)
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

