using System;
using System.Threading.Tasks;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Forte.EpiserverRedirects.Menu
{
    public class QueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;
            var queryPropertiesDictionary = request.Query;

            try
            {
                var model = new Query
                {
                    OldPattern = queryPropertiesDictionary["oldPattern"],
                    NewPattern = queryPropertiesDictionary["newPattern"],
                    ContentId = Parser.ParseContentIdNullable(queryPropertiesDictionary["contentId"]),
                    RedirectType = Parser.ParseRedirectTypeNullable(queryPropertiesDictionary["redirectType"]),
                    RedirectRuleType = Parser.ParseRedirectRuleTypeNullable(queryPropertiesDictionary["redirectRuleType"]),
                    RedirectOrigin = Parser.ParseRedirectOriginNullable(queryPropertiesDictionary["redirectOrigin"]),
                    IsActive = Parser.ParseNullableBoolean(queryPropertiesDictionary["isActive"]),
                    CreatedOnFrom = Parser.ParseCreatedOnFrom(queryPropertiesDictionary["createdOnFrom"]),
                    CreatedOnTo = Parser.ParseCreatedOnTo(queryPropertiesDictionary["createdOnTo"]),
                    CreatedBy = queryPropertiesDictionary["createdBy"],
                    Notes = queryPropertiesDictionary["notes"],
                    Priority = Parser.ParsePriorityNullable(queryPropertiesDictionary["priority"]),
                    SortColumns = Parser.ParseSortColumns(request.QueryString.Value),
                    Range = ItemRange.ReadHeaderFrom(request)
                };

                bindingContext.Result = ModelBindingResult.Success(model);

                return Task.CompletedTask;
            }
            catch
            {
                throw new Exception("Failed to parse query string from http request");
            }
        }
    }
}
