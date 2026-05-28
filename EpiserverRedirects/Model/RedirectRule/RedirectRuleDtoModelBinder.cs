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
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var jsonBody = await GetBody(bindingContext.HttpContext.Request);
            var redirectRulesDto = JsonConvert.DeserializeObject<RedirectRulesRequestDto>(jsonBody);

            var validationResult = new List<ValidationResult>();
            var mappedRedirectRules = new List<RedirectRuleDto>();

            var operation = Parser.ParseRedirectRuleOperation(redirectRulesDto.Operation);

            if (operation == RedirectRuleOperation.None)
            {
                bindingContext.ModelState.AddModelError(nameof(RedirectRulesRequestDto.Operation), "Operation is None or cannot be parsed successfully. Expected operations: create, update");
                bindingContext.Result = ModelBindingResult.Failed();
            }

            foreach (var redirectRuleDtoProperties in redirectRulesDto.RedirectRules)
            {
                try
                {
                    var redirectRule = new RedirectRuleDto
                    {
                        //TODO: no id passing
                        Id = Parser.ParseIdentity(redirectRuleDtoProperties),
                        OldPattern = redirectRuleDtoProperties["oldPattern"],
                        NewPattern = redirectRuleDtoProperties["newPattern"],
                        ContentId = Parser.ParseContentIdNullable(redirectRuleDtoProperties["contentId"]),
                        ContentProviderId =
                            Parser.ParseContentProviderIdNullable(redirectRuleDtoProperties["contentProviderKey"]),
                        RedirectType = Parser.ParseRedirectType(redirectRuleDtoProperties["redirectType"]),
                        Priority = Parser.ParsePriorityNullable(redirectRuleDtoProperties["priority"]),
                        RedirectRuleType = Parser.ParseRedirectRuleType(redirectRuleDtoProperties["redirectRuleType"]),
                        IsActive = Parser.ParseBoolean(redirectRuleDtoProperties["isActive"]),
                        Notes = redirectRuleDtoProperties["notes"],
                        HostId = Parser.ParseHostIdNullable(redirectRuleDtoProperties["hostId"]),
                    };

                    if (!Validator.TryValidateObject(redirectRule, new ValidationContext(redirectRule), validationResult, true))
                    {
                        continue;
                    }
                    
                    mappedRedirectRules.Add(redirectRule);
                }
                catch
                {
                    throw new Exception("Failed to parse json from http request body " + jsonBody);
                }
            }

            if (validationResult.Count == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(new RedirectRulesDto
                {
                    Operation = operation,
                    RedirectRules = mappedRedirectRules,
                });

                return;
            }

            foreach (var vr in validationResult)
            {
                bindingContext.ModelState.AddModelError(string.Join(",", vr.MemberNames), vr.ErrorMessage);
            }

            bindingContext.Result = ModelBindingResult.Failed();
        }

        private static async Task<string> GetBody(HttpRequest request)
        {
            using var reader = new StreamReader(request.Body);

            return await reader.ReadToEndAsync();
        }
    }
}
