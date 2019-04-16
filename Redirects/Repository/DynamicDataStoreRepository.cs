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
        
        public override RedirectRule GetRedirectRule(UrlPath oldPath)
        {
            var redirect = _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.OldPath == oldPath);

            return redirect;
        }

        public override IQueryable<RedirectRule> GetAllRedirectRules()
        {
            return _dynamicDataStore.Items<RedirectRule>().AsQueryable();
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