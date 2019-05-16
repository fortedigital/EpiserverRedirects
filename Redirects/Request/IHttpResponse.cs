namespace Forte.Redirects.Request
{
    public interface IHttpResponse
    {
        void Redirect(string location, int statusCode);
    }
}