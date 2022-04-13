using System;
using System.Collections.Generic;
using EPiServer.Shell.Services.Rest;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public static class Parser
    {
        public static RedirectType ParseRedirectType(string value)
        {
            Enum.TryParse<RedirectType>(value, out var redirectType);
            return redirectType;
        }
        
        public static RedirectType? ParseRedirectTypeNullable(string value)
        {
            if(string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }

            Enum.TryParse<RedirectType>(value, out var redirectType);
            return redirectType;
        }

        public static RedirectRuleType ParseRedirectRuleType(string value)
        {
            Enum.TryParse<RedirectRuleType>(value, out var redirectRuleType);
            return redirectRuleType;
        }

        public static RedirectRuleType? ParseRedirectRuleTypeNullable(string value)
        {
            if(string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }

            Enum.TryParse<RedirectRuleType>(value, out var redirectRuleType);
            return redirectRuleType;
        }

        public static Guid? ParseIdentity(Dictionary<string, string> redirectRuleDtoProperties)
        {
            if (!redirectRuleDtoProperties.TryGetValue("id", out var guidString))
            {
                return null;
            }

            if(Guid.TryParse(guidString, out var guid))
            {
                return guid;
            }

            return null;
        }

        public static bool ParseBoolean(string redirectRuleDtoProperty)
        {
            switch (redirectRuleDtoProperty)
            {
                case "0":
                    return false;
                case "1":
                    return true;
                default:
                    return bool.Parse(redirectRuleDtoProperty);
            }
        }
        
        public static bool? ParseNullableBoolean(string value)
        {
            if (bool.TryParse(value, out var isActive))
            {
                return isActive;
            }

            return null;
        }

        public static DateTime? ParseCreatedOnFrom(string value)
        {
            if(DateTime.TryParse(value, out var createdOnFrom))
            {
                return createdOnFrom.ToUniversalTime();
            }

            return null;
        }

        public static DateTime? ParseCreatedOnTo(string value)
        {
            if(DateTime.TryParse(value, out var createdOnTo))
            {
                return createdOnTo.ToUniversalTime();
            }

            return null;
        }

        public static IEnumerable<SortColumn> ParseSortColumns(string sortQuery)
        {
            return string.IsNullOrEmpty(sortQuery)
                ? null 
                : SortColumn.Parse(sortQuery);
        }

        public static RedirectOrigin ParseRedirectOrigin(string value)
        {
            Enum.TryParse<RedirectOrigin>(value, out var redirectOrigin);
            return redirectOrigin;
        }
        
        public static RedirectOrigin? ParseRedirectOriginNullable(string value)
        {
            if(string.IsNullOrEmpty(value) || value == "0")
            {
                return null;
            }

            Enum.TryParse<RedirectOrigin>(value, out var redirectOrigin);
            return redirectOrigin;
        }

        public static int? ParseContentIdNullable(string value) => ParseNullableInt(value);
        public static int? ParsePriorityNullable(string value) => ParseNullableInt(value);
        

        private static int? ParseNullableInt(string value)
        {
            return int.TryParse(value, out var intValue)
                ? intValue
                : (int?)null;
        }
    }
}