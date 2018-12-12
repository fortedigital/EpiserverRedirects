using System;
using System.Linq;

namespace ForteDigital.UrlRedirects.UrlRewritePlugin
{
    public interface IUrlRedirectsService
    {
        IQueryable<UrlRewriteModel> GetAll();
        UrlRedirectsDto Post(UrlRedirectsDto urlRedirectsDto);
        UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto);
        void Delete(Guid id);
    }
}
