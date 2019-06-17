using System;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
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
                .Any(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlreadyExist)
            {
                throw new ApplicationException($"Redirect with this oldUrl: {urlRedirectsDto.OldUrl} already exist");
            }

            var identity = store.Save(urlRewriteModel);

            urlRewriteModel.Id = identity;

            return urlRewriteModel.MapToUrlRedirectsDto();
        }

        public UrlRedirectsDto Put(UrlRedirectsDto urlRedirectsDto)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteModel = urlRedirectsDto.MapToUrlRewriteModel();

            var existingRedirect = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            var newIdentity = store.Save(urlRewriteModel, existingRedirect?.Id);
            urlRewriteModel.Id = newIdentity.ExternalId;
            
            return urlRewriteModel.MapToUrlRedirectsDto();
        }
    }
}