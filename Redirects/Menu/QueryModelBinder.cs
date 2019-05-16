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
                    oldPattern = queryPropertiesDictionary["oldPattern"],
                    newPattern = queryPropertiesDictionary["newPattern"],
                    redirectType = ParseRedirectType(queryPropertiesDictionary["redirectType"]),
                    redirectRuleType = ParseRedirectRuleType(queryPropertiesDictionary["redirectRuleType"]),
                    SortColumns = ParseSortColumns(queryPropertiesDictionary[""]),
                };
            }
            catch
            {
                throw new Exception("Failed to parse query string from http request");
            }
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