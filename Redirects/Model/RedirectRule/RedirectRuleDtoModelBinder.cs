using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EPiServer.Data;
using Newtonsoft.Json;

namespace Forte.Redirects.Model.RedirectRule
{
    public class RedirectRuleDtoModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var jsonBody = GetBody(controllerContext.HttpContext.Request);
            var redirectRuleDtoProperties = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonBody);

            try
            {
                return new RedirectRuleDto
                {
                    //TODO: no id passing
                    Id = ParseIdentity(redirectRuleDtoProperties["identity"]),
                    OldPattern = redirectRuleDtoProperties["oldPattern"],
                    NewPattern = redirectRuleDtoProperties["newPattern"],
                    RedirectType = ParseRedirectType(redirectRuleDtoProperties["redirectType"]),
                    RedirectRuleType = ParseRedirectRuleType(redirectRuleDtoProperties["redirectRuleType"]),
                    IsActive = ParseIsActive(redirectRuleDtoProperties["isActive"]),
                };
            }
            catch
            {
                throw new Exception("Failed to parse json from http request body " + jsonBody);
            }
        }

        private static Guid? ParseIdentity(string guidString)
        {
            if(Guid.TryParse(guidString, out var guid))
                return guid;
            return null;
        }

        private static bool ParseIsActive(string redirectRuleDtoProperty)
        {
            return bool.Parse(redirectRuleDtoProperty);
        }

        private static string GetBody(HttpRequestBase request)
        {
            var inputStream = request.InputStream;
            inputStream.Position = 0;

            using (var reader = new StreamReader(inputStream))
            {
                var body = reader.ReadToEnd();
                return body;
            }
        }

        private static RedirectType.RedirectType ParseRedirectType(string val)
        {
            Enum.TryParse<RedirectType.RedirectType>(val, out var redirectType);
            return redirectType;
        }

        private static RedirectRuleType ParseRedirectRuleType(string val)
        {
            Enum.TryParse<RedirectRuleType>(val, out var redirectRuleType);
            return redirectRuleType;
        }
    }
}