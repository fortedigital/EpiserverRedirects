using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Menu
{
    public class QueryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var queryPropertiesDictionary = controllerContext.HttpContext.Request.QueryString;

            try
            {
                return new Query
                {
                    OldPattern = queryPropertiesDictionary["oldPattern"],
                    NewPattern = queryPropertiesDictionary["newPattern"],
                    RedirectType = ParseRedirectType(queryPropertiesDictionary["redirectType"]),
                    RedirectRuleType = ParseRedirectRuleType(queryPropertiesDictionary["redirectRuleType"]),
                    IsActive = ParseIsActive(queryPropertiesDictionary["isActive"]),
                    CreatedOnFrom = ParseCreatedOnFrom(queryPropertiesDictionary["createdOnFrom"]),
                    CreatedOnTo = ParseCreatedOnTo(queryPropertiesDictionary["createdOnTo"]),
                    CreatedBy = queryPropertiesDictionary["createdBy"],
                    Notes = queryPropertiesDictionary["notes"],
                    SortColumns = ParseSortColumns(queryPropertiesDictionary.ToString()),
                };
            }
            catch
            {
                throw new Exception("Failed to parse query string from http request");
            }
        }

        private static DateTime? ParseCreatedOnFrom(string value)
        {
            if(DateTime.TryParse(value, out var createdOnFrom))
                return createdOnFrom;
            return null;
        }
        
        private static DateTime? ParseCreatedOnTo(string value)
        {
            if(DateTime.TryParse(value, out var createdOnTo))
                return createdOnTo;
            return null;
        }

        private static bool? ParseIsActive(string value)
        {
            if (bool.TryParse(value, out var isActive))
                return isActive;
            return null;
        }

        private static IEnumerable<SortColumn> ParseSortColumns(string sortQuery)
        {
            return string.IsNullOrEmpty(sortQuery)
                ? null 
                : SortColumn.Parse(sortQuery);
        }

        private static RedirectType? ParseRedirectType(string val)
        {
            if(string.IsNullOrEmpty(val) || val == "0")
                return null;

            Enum.TryParse<RedirectType>(val, out var redirectType);
            return redirectType;
        }

        private static RedirectRuleType? ParseRedirectRuleType(string val)
        {
            if(string.IsNullOrEmpty(val) || val == "0")
                return null;

            Enum.TryParse<RedirectRuleType>(val, out var redirectRuleType);
            return redirectRuleType;
        }
    }
}