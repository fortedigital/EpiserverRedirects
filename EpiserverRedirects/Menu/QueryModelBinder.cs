using System;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Menu
{
    public class QueryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var queryPropertiesDictionary = request.QueryString;

            try
            {
                return new Query
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
                    SortColumns = Parser.ParseSortColumns(queryPropertiesDictionary.ToString()),
                    Range = ItemRange.ReadHeaderFrom(request)
                };
            }
            catch
            {
                throw new Exception("Failed to parse query string from http request");
            }
        }

    }
}