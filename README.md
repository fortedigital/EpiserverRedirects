# EPiAutoAlias for EPiServer

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
   <system.web> 
    <httpModules> 
     <add type= "Forte.EpiserverRedirects.AspNet.HttpModule, EpiserverRedirects" name="HttpModule" /> 
    </httpModules> 
   </system.web> 
  </configuration>
  ```

Manage Redirections
------------
There are two ways to manage redirections:
1. **Using user interface**
2. **Using public interface**

a) For version 1.x.x, using ```IUrlRedirectsServoce```:

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
