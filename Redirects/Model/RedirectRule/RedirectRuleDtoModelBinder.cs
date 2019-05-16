using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
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
                    //Id = Identity.Parse(redirectRuleDtoProperties["id"]),
                    OldPattern = redirectRuleDtoProperties["oldPattern"],
                    NewPattern = redirectRuleDtoProperties["newPattern"],
                    RedirectType = ParseRedirectType(redirectRuleDtoProperties["oldPattern"]),
                    RedirectRuleType = ParseRedirectRuleType(redirectRuleDtoProperties["oldPattern"]),
                    //IsActive = ParseIsActive(redirectRuleDtoProperties["isActive"]),
                };
            }
            catch
            {
                throw new Exception("Failed to parse json from http request body " + jsonBody);
            }
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