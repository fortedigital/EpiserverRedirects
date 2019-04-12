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
    }
}