namespace Forte.RedirectMiddleware.Response.HttpResponse
{
    public interface IHttpResponse
    {
        void Redirect(string location, int statusCode);
    }
}