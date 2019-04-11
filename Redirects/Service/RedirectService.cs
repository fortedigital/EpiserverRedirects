using System;
using System.Linq;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Service
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectService))]
    public class RedirectService : IRedirectService
    {
        private readonly IRepository _repository;
        public RedirectService(IRepository repository)
        {
            _repository = repository;
        }

        public RedirectModel GetRedirect(string oldPath)
        {
            oldPath = RedirectModel.NormalizePath(oldPath);
            return _repository.GetRedirect(oldPath);
        }
        
        public IQueryable<RedirectModel> GetAllRedirects()
        {
            return _repository.GetAllRedirects();
        }

        public RedirectModel CreateRedirect(RedirectModel redirectVM)
        {
            return redirectVM.Validate()
                ? _repository.CreateRedirect(redirectVM)
                : null;
        }

        public RedirectModel UpdateRedirect(RedirectModel redirectVM)
        {
            return redirectVM.Validate()
                ? _repository.UpdateRedirect(redirectVM)
                : null;
        }
        
        public bool DeleteRedirect(Guid id)
        {
            return _repository.DeleteRedirect(id);
        }
        
    }

    public interface IRedirectService
    {
        RedirectModel GetRedirect(string oldPath);
        IQueryable<RedirectModel> GetAllRedirects();
        RedirectModel CreateRedirect(RedirectModel redirectVM);
        RedirectModel UpdateRedirect(RedirectModel redirectVM);
        bool DeleteRedirect(Guid id);
    }
}