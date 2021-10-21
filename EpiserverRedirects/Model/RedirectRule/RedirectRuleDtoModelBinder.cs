using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleDtoModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var jsonBody = GetBody(controllerContext.HttpContext.Request);
            var redirectRuleDtoProperties = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonBody);

            try
            {
                var redirectRule = new RedirectRuleDto
                {
                    //TODO: no id passing
                    Id = Parser.ParseIdentity(redirectRuleDtoProperties),
                    OldPattern = redirectRuleDtoProperties["oldPattern"],
                    NewPattern = redirectRuleDtoProperties["newPattern"],
                    ContentId = Parser.ParseContentIdNullable(redirectRuleDtoProperties["contentId"]),
                    RedirectType = Parser.ParseRedirectType(redirectRuleDtoProperties["redirectType"]),
                    Priority = Parser.ParsePriorityNullable(redirectRuleDtoProperties["priority"]),
                    RedirectRuleType = Parser.ParseRedirectRuleType(redirectRuleDtoProperties["redirectRuleType"]),
                    IsActive = Parser.ParseBoolean(redirectRuleDtoProperties["isActive"]),
                    Notes = redirectRuleDtoProperties["notes"]
                };

                var validationResult = new List<ValidationResult>();
                if (Validator.TryValidateObject(redirectRule, new ValidationContext(redirectRule), validationResult, true))
                {
                    return redirectRule;
                }
                
                foreach (var vr in validationResult)
                {
                    bindingContext.ModelState.AddModelError(string.Join(",", vr.MemberNames), vr.ErrorMessage);
                }

                return redirectRule;
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
    }
}
