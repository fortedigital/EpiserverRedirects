using System;
using System.Net;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Forte.EpiserverRedirects.Middleware
{
    public class RedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RequestHandler requestHandler)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                var requestUri = new Uri(context.Request.GetEncodedUrl());
                var response = new RedirectHttpResponse(context.Response);
                await requestHandler.Invoke(requestUri, response);
            }
        }
    }
}