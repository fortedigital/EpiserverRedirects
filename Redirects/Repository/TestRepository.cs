using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;

namespace Redirects.Repository
{
    public class TestRepository : Repository
    {
        private readonly Dictionary<Guid, RedirectModel> _redirectsDictionary;

        public TestRepository(Dictionary<Guid, RedirectModel> redirectsCollection)
        {
            _redirectsDictionary = redirectsCollection;
        }

        public override RedirectModel GetRedirect(string oldPath)
        {
            return _redirectsDictionary.FirstOrDefault(r => r.Value.OldPath == oldPath).Value;
        }

        public override IQueryable<RedirectModel> GetAllRedirects()
        {
            return _redirectsDictionary.Select(r=>r.Value).AsQueryable();
        }

        public override RedirectModel CreateRedirect(RedirectModel redirectVM)
        {
            redirectVM.Id = Identity.NewIdentity();
            _redirectsDictionary.Add(redirectVM.Id.ExternalId, redirectVM);
            return redirectVM;
        }

        public override RedirectModel UpdateRedirect(RedirectModel redirectVM)
        {
            _redirectsDictionary.TryGetValue(redirectVM.Id.ExternalId, out var existingRedirect);
            
            if(existingRedirect==null)
                throw new KeyNotFoundException("No existing redirect with this GUID");
            
            MapViewModelToRedirect(redirectVM, existingRedirect);
            return existingRedirect;
        }

        public override bool DeleteRedirect(Guid id)
        {
            return _redirectsDictionary.Remove(id);
        }
    }
}