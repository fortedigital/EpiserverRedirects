using System;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;

namespace Forte.RedirectMiddleware.Repository
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleRepository))]
    public class DynamicDataStoreRedirectRuleRepository : RedirectRuleRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly DynamicDataStore _dynamicDataStore;

        public DynamicDataStoreRedirectRuleRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _dynamicDataStore = CreateStore();
        }
        
        private DynamicDataStore CreateStore()
        {
            return _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));
        }
        
        public override RedirectRuleDto GetRedirect(UrlPath oldPath)
        {
            var redirect = _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.OldPath == oldPath);
            var redirectDto = RedirectRuleMapper.ModelToDto(redirect);
            return redirectDto;
        }

        public override IQueryable<RedirectRuleDto> GetAllRedirects()
        {
            //TODO: betterMapping
            return _dynamicDataStore.Items<RedirectRule>().Select(r=>RedirectRuleMapper.ModelToDto(r)).AsQueryable();
        }

        public override RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            var redirect = RedirectRuleMapper.DtoToModel(redirectRuleDTO);
            
            var newRedirectIdentity = _dynamicDataStore.Save(redirect);

            redirectRuleDTO.Id = newRedirectIdentity;
            return redirectRuleDTO;
        }

        public override RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            var redirect = _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.Id == redirectRuleDTO.Id);
            
            if(redirect==null)
                throw new Exception("No existing redirect with this GUID");
            
            RedirectRuleMapper.DtoToModel(redirectRuleDTO, redirect);
            
            _dynamicDataStore.Save(redirect);

            return redirectRuleDTO;
        }

        public override bool DeleteRedirect(Guid id)
        {
            try
            {
                _dynamicDataStore.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}