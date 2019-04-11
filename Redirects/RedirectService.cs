using System;
using System.Linq;
using Redirects.Repository;

namespace Redirects
{
    public class RedirectService
    {
        private readonly IRepository _repository;
        public RedirectService(IRepository repository)
        {
            _repository = repository;
        }

        public RedirectModel GetRedirect(string oldPath)
        {
            return _repository.GetRedirect(oldPath);
        }
        
        public IQueryable<RedirectModel> GetAllRedirects()
        {
            return _repository.GetAllRedirects();
        }

        public RedirectModel CreateRedirect(RedirectModel redirectVM)
        {
            return _repository.CreateRedirect(redirectVM);
        }
        
        public RedirectModel UpdateRedirect(RedirectModel redirectVM)
        {
            return _repository.UpdateRedirect(redirectVM);
        }
        
        public bool DeleteRedirect(Guid id)
        {
            return _repository.DeleteRedirect(id);
        }
        
    }
}