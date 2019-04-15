using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Service;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private const int NotFoundStatusCode = 404;
        private const string LocationHeader = "Location";
        private Injected<IRedirectService> RedirectService  { get; set; }
        public RedirectMiddleware(OwinMiddleware next) : base(next) {}

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == NotFoundStatusCode)
            {
                var originalRequestPath = context.Request.Uri.AbsolutePath;
                var redirectRuleDto = RedirectService.Service.GetRedirect(originalRequestPath);

                if (redirectRuleDto != null)
                    RedirectResponse(context, redirectRuleDto);
            }
        }

        private static void RedirectResponse(IOwinContext context, RedirectRuleDto redirectRuleDto)
        {
            context.Response.StatusCode = Http_1_0_RedirectTypeMapper.MapToHttpResponseCode(redirectRuleDto.RedirectType);
            context.Response.Headers.Set(LocationHeader, redirectRuleDto.NewUrl);
        }
    }
}