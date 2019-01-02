# EPiAutoAlias for EPiServer

Basic installation scenario
------------
1. The package can be found in official NuGet repository.
```Install-Package Forte.EpiserverRedirects``` 

2. Configure module in web.config

```xml
  <episerver.shell>
    <protectedModules rootPath="~/EPiServer/">
      <add name="EpiserverRedirects">
        <assemblies>
          <add assembly="Forte.EpiserverRedirects" />
        </assemblies>
      </add>
    </protectedModules>
  </episerver.shell>
```

3. Add ```UrlRewriteMiddleware``` into Owin Pipeline in ```Startup``` class

```c#
  app.Use(typeof(UrlRewriteMiddleware));
```

Manage Redirections
------------
There are two ways to manage rediractions:
1. Using ```IUrlRedirectsService```

```c#
    public interface IUrlRedirectsService
    {
        IQueryable<UrlRewriteModel> GetAll();
        UrlRedirectsDto Post(UrlRedirectsDto urlRedirectsDto);
        UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto);
        void Delete(Guid id);
    }
```
2. Using user interface 

Configuration
-------------

There is possibility to disable adding redirects to moved content automatically. To do this, set `Forte.EpiserverRedirects.UrlRewritePlugin.Configuration.AddAutomaticRedirects` flag to `false`.

