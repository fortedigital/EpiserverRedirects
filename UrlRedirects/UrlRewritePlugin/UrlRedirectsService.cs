using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UrlRedirects.UrlRewritePlugin;

namespace Test.modules.UrlRedirects.UrlRewritePlugin
{
    [ServiceConfiguration(ServiceType = typeof(IUrlRedirectsService))]
    public class UrlRedirectsService : IUrlRedirectsService
    {
        private readonly DynamicDataStoreFactory dynamicDataStoreFactory;

        public UrlRedirectsService(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            this.dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        public void Delete(Guid id)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            store.Delete(id);
        }

        public IQueryable<UrlRewriteModel> GetAll()
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            return store.Items<UrlRewriteModel>().AsQueryable();
        }

        public UrlRedirectsDto Post(UrlRedirectsDto urlRedirectsDto)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteModel = urlRedirectsDto.MapToUrlRewriteModel();

            var redirectAlreadyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlreadyExist != null)
            {
                throw new ApplicationException($"Redirect with this oldUrl: {urlRedirectsDto.OldUrl} already exist");
            }

            var identity = store.Save(urlRewriteModel);

            urlRewriteModel.Id = identity;

            return urlRewriteModel.MapToUrlRedirectsDtoModel();
        }

        public UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteModel = urlRedirectsDto.MapToUrlRewriteModel();

            var redirectAlreadyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl && x.Id.ExternalId != urlRedirectsDto.Id);

            if (redirectAlreadyExist != null)
            {
                throw new ApplicationException($"Redirect with this oldUrl: {urlRedirectsDto.OldUrl} already exist");
            }

            store.Save(urlRewriteModel, urlRedirectsDto.Id);

            return urlRewriteModel.MapToUrlRedirectsDtoModel();
        }
    }
}