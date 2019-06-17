using System;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    public static class UrlRedirectsModelMapper
    {
        public static UrlRedirectsDto MapToUrlRedirectsDto(this UrlRewriteModel urlRewriteModel)
        {
            if (!Enum.TryParse(urlRewriteModel.Type, out UrlRedirectsType urlRedirectsType)) { throw new ArgumentException("Invalid UrlRedirects Type"); }

            return new UrlRedirectsDto(
                urlRewriteModel.Id.ExternalId, urlRewriteModel.OldUrl.NormalizePath(),
                urlRewriteModel.NewUrl ?? RedirectHelper.GetRedirectUrl(urlRewriteModel.ContentId),
                urlRewriteModel.ContentId, urlRedirectsType, urlRewriteModel.Priority,
                (RedirectStatusCode)urlRewriteModel.RedirectStatusCode);
        }

        public static UrlRewriteModel MapToUrlRewriteModel(this UrlRedirectsDto urlRedirectsDtoModel)
        {
            var urlRewriteModel = new UrlRewriteModel
            {
                OldUrl = urlRedirectsDtoModel.OldUrl.NormalizePath(),
                NewUrl = urlRedirectsDtoModel.NewUrl,
                ContentId = urlRedirectsDtoModel.ContentId,
                Type = urlRedirectsDtoModel.Type.ToString(),
                Priority = urlRedirectsDtoModel.Priority,
                RedirectStatusCode = (int)urlRedirectsDtoModel.RedirectStatusCode
            };

            if (Guid.Empty != urlRedirectsDtoModel.Id)
            {
                urlRewriteModel.Id = urlRedirectsDtoModel.Id;
            }

            return urlRewriteModel;
        }
    }
}