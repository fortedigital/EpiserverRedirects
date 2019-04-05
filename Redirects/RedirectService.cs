namespace Redirects
{
    public class RedirectService
    {
        private readonly IRepository _repository;
        public RedirectService(IRepository repository)
        {
            _repository = repository;
        }

        public string FindRedirect(string oldPath)
        {
            var newPath = _repository.FindNewPath(oldPath);

            return newPath;
        }
    }
}