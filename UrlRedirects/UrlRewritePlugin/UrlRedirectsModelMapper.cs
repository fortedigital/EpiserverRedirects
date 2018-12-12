using System;

namespace Forte.UrlRedirects.UrlRewritePlugin
{
    public static class UrlRedirectsModelMapper
    {
        public static UrlRedirectsDto MapToUrlRedirectsDtoModel(this UrlRewriteModel urlRewriteModel)
        {
            if(!Enum.TryParse(urlRewriteModel.Type, out UrlRedirectsType urlRedirectsType)) { throw new ArgumentException("Invalid UrlRedirects Type"); }

            return new UrlRedirectsDto()
            {
                Id = urlRewriteModel.Id.ExternalId,
                OldUrl = urlRewriteModel.OldUrl,
                NewUrl = urlRewriteModel.NewUrl ?? RedirectHelper.GetRedirectUrl(urlRewriteModel.ContentId),
                ContentId = urlRewriteModel.ContentId,
                Type = urlRedirectsType,
                Priority = urlRewriteModel.Priority,
                RedirectStatusCode = (RedirectStatusCode)urlRewriteModel.RedirectStatusCode
            };
        }

        public static UrlRewriteModel MapToUrlRewriteModel(this UrlRedirectsDto urlRedirectsDtoModel)
        {
            var urlRewriteModel = new UrlRewriteModel()
            {
                OldUrl = urlRedirectsDtoModel.OldUrl,
                NewUrl = urlRedirectsDtoModel.NewUrl,
                ContentId = urlRedirectsDtoModel.ContentId,
                Type = urlRedirectsDtoModel.Type.ToString(),
                Priority = urlRedirectsDtoModel.Priority,
                RedirectStatusCode = (int)urlRedirectsDtoModel.RedirectStatusCode
            };

            if(Guid.Empty != urlRedirectsDtoModel.Id)
            {
                urlRewriteModel.Id = urlRedirectsDtoModel.Id;
            }

            return urlRewriteModel;
        }
    }
}