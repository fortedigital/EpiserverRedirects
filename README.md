# EPiAutoAlias for EPiServer

Basic installation scenario
------------
1. ```Install-Package ...```
2. Configure module in web.config

```xml
  <episerver.shell>
    <protectedModules rootPath="~/EPiServer/">
      <add name="UrlRedirects">
        <assemblies>
          <add assembly="UrlRedirects" />
        </assemblies>
      </add>
    </protectedModules>
  </episerver.shell>
```

3. Add ```UrlRewriteMiddleware``` into Owin Pipeline in ```Startup``` class

```c#
  app.Use(typeof(UrlRewriteMiddleware));
```

Manage Rediractions
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


