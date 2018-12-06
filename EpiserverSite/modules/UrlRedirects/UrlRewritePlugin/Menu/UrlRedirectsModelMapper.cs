using UrlRedirects.UrlRewritePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlRedirects.UrlRewritePlugin.Menu
{
    public static class UrlRedirectsModelMapper
    {
        public static UrlRedirectsMenuViewModel MapToUrlRedirectsMenuViewModel(this UrlRewriteModel urlRewriteModel)
        {
            return new UrlRedirectsMenuViewModel()
            {
                Id = urlRewriteModel.Id.ExternalId,
                OldUrl = urlRewriteModel.OldUrl,
                NewUrl = urlRewriteModel.NewUrl ?? RedirectHelper.GetRedirectUrl(urlRewriteModel.ContentId),
                Type = urlRewriteModel.Type,
                Priority = urlRewriteModel.Priority,
                RedirectStatusCode = urlRewriteModel.RedirectStatusCode
            };
        }

        public static UrlRewriteModel MapToUrlRewriteModel(this UrlRedirectsMenuViewModel urlRedirectsMenuViewModel)
        {
            return new UrlRewriteModel()
            {
                OldUrl = urlRedirectsMenuViewModel.OldUrl,
                NewUrl = urlRedirectsMenuViewModel.NewUrl,
                Type = urlRedirectsMenuViewModel.Type,
                Priority = urlRedirectsMenuViewModel.Priority,
                RedirectStatusCode = urlRedirectsMenuViewModel.RedirectStatusCode
            };
        }
    }
}