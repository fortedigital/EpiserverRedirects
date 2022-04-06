using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleDtoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var jsonBody = GetBody(bindingContext.HttpContext.Request);
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
                    bindingContext.Result = ModelBindingResult.Success(redirectRule);
                    return Task.CompletedTask;
                }

                foreach (var vr in validationResult)
                {
                    bindingContext.ModelState.AddModelError(string.Join(",", vr.MemberNames), vr.ErrorMessage);
                }

                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            catch
            {
                throw new Exception("Failed to parse json from http request body " + jsonBody);
            }
        }

        private static string GetBody(HttpRequest request)
        {
            var inputStream = request.Body;
            inputStream.Position = 0;

            using var reader = new StreamReader(inputStream);
            var body = reader.ReadToEnd();

            return body;
        }
    }
}
