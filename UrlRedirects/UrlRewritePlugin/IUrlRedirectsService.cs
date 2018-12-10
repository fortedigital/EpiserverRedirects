using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlRedirects.UrlRewritePlugin;

namespace Test.modules.UrlRedirects.UrlRewritePlugin
{
    public interface IUrlRedirectsService
    {
        IQueryable<UrlRewriteModel> GetAll();
        UrlRedirectsDto Post(UrlRedirectsDto urlRedirectsDto);
        UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto);
        void Delete(Guid id);
    }
}
