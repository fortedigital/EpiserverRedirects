using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Model.RedirectRule;

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
                    RedirectType = Parser.ParseRedirectTypeNullable(queryPropertiesDictionary["redirectType"]),
                    RedirectRuleType = Parser.ParseRedirectRuleTypeNullable(queryPropertiesDictionary["redirectRuleType"]),
                    RedirectOrigin = Parser.ParseRedirectOriginNullable(queryPropertiesDictionary["redirectOrigin"]),
                    IsActive = Parser.ParseIsActiveNullable(queryPropertiesDictionary["isActive"]),
                    CreatedOnFrom = Parser.ParseCreatedOnFrom(queryPropertiesDictionary["createdOnFrom"]),
                    CreatedOnTo = Parser.ParseCreatedOnTo(queryPropertiesDictionary["createdOnTo"]),
                    CreatedBy = queryPropertiesDictionary["createdBy"],
                    Notes = queryPropertiesDictionary["notes"],
                    SortColumns = Parser.ParseSortColumns(queryPropertiesDictionary.ToString()),
                };
            }
            catch
            {
                throw new Exception("Failed to parse query string from http request");
            }
        }

        
    }
}