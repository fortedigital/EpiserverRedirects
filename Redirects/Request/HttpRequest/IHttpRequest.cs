using System;

namespace Forte.RedirectMiddleware.Request.HttpRequest
{
    public interface IHttpRequest
    {
        Uri Url { get; }
    }
}