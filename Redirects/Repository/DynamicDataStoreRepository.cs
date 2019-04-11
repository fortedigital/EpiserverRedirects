using System;
using System.Linq;
using EPiServer.Data.Dynamic;

namespace Redirects.Repository
{
    public class DynamicDataStoreRepository : Repository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly DynamicDataStore _dynamicDataStore;

        public DynamicDataStoreRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _dynamicDataStore = CreateStore();
        }
        
        private DynamicDataStore CreateStore()
        {
            return _dynamicDataStoreFactory.CreateStore(typeof(RedirectModel));
        }
        
        public override RedirectModel GetRedirect(string oldPath)
        {
            return _dynamicDataStore.Items<RedirectModel>().FirstOrDefault(r => r.OldPath == oldPath);
        }

        public override IQueryable<RedirectModel> GetAllRedirects()
        {
            return _dynamicDataStore.Items<RedirectModel>().AsQueryable();
        }

        public override RedirectModel CreateRedirect(RedirectModel redirectVM)
        {
            var newRedirect = new RedirectModel();
            MapViewModelToRedirect(redirectVM, newRedirect);
            
            var newRedirectIdentity = _dynamicDataStore.Save(newRedirect);

            redirectVM.Id = newRedirectIdentity;
            return redirectVM;
        }

        public override RedirectModel UpdateRedirect(RedirectModel redirectVM)
        {
            var existingRedirect = _dynamicDataStore.Items<RedirectModel>().FirstOrDefault(r => r.Id == redirectVM.Id);
            
            if(existingRedirect==null)
                throw new Exception("No existing redirect with this GUID");
            MapViewModelToRedirect(redirectVM, existingRedirect);
            
            _dynamicDataStore.Save(existingRedirect);

            return redirectVM;
        }

        public override bool DeleteRedirect(Guid id)
        {
            try
            {
                _dynamicDataStore.Delete(id);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}