using System;

namespace Forte.RedirectMiddleware.Model
{
    public static class RedirectRuleValidator
    {
        public static bool ValidateDto(RedirectRuleDto redirectRuleDto)
        {
            try
            {
                var trimmedOldPath = redirectRuleDto.OldPath.Trim();
                new Uri(trimmedOldPath, UriKind.Relative);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Entered path is not a valid relative path: " + e);
                return false;
            }
        }
    }
}