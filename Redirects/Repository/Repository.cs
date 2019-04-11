using System;
using System.Linq;
using Forte.RedirectMiddleware.Model;

namespace Forte.RedirectMiddleware.Repository
{
    public interface IRepository
    {
        RedirectModel GetRedirect(string oldPath);
        IQueryable<RedirectModel> GetAllRedirects();
        RedirectModel CreateRedirect(RedirectModel redirectVM);
        RedirectModel UpdateRedirect(RedirectModel redirectVM);
        bool DeleteRedirect(Guid id);
    }

    public abstract class Repository : IRepository
    {
        public abstract RedirectModel GetRedirect(string oldPath);
        public abstract IQueryable<RedirectModel> GetAllRedirects();
        public abstract RedirectModel CreateRedirect(RedirectModel redirectVM);
        public abstract RedirectModel UpdateRedirect(RedirectModel redirectVM);
        public abstract bool DeleteRedirect(Guid id);
        
        protected static void MapViewModelToRedirect(RedirectModel redirectVM, RedirectModel redirect)
        {
            redirect.NewUrl = redirectVM.NewUrl;
            redirect.OldPath = RedirectModel.NormalizePath(redirectVM.OldPath);
            redirect.StatusCode = redirectVM.StatusCode;
            redirect.IsActive = redirectVM.IsActive;
        }
    }
}