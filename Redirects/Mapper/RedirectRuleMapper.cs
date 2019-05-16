using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.UrlPath;

namespace Forte.Redirects.Mapper
{
    public class RedirectRuleMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleMapper() : base(ModelToDtoDelegate, DtoToModelDelegate)
        {
        }
        

        private static RedirectRuleDto ModelToDtoDelegate(RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            destination.Id = source.Id;
            destination.NewPattern = source.NewPattern;
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;
            
            destination.RedirectRuleType = source.RedirectRuleType;

            switch (destination.RedirectRuleType)
            {
                case RedirectRuleType.ExactMatch:
                    destination.OldPattern = source.OldPath.Path.OriginalString;
                    break;
                case RedirectRuleType.Regex:
                    destination.OldPattern = source.OldPattern;
                    break;
                case RedirectRuleType.Wildcard:
                    break;
            }

            return destination;
        }

        public static RedirectRule DtoToModelDelegate(RedirectRuleDto source)
        {
            var destination = new RedirectRule();
            destination.Id = source.Id;
            destination.NewPattern = source.NewPattern;

            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;
            destination.RedirectRuleType = source.RedirectRuleType;

            switch (destination.RedirectRuleType)
            {
                case RedirectRuleType.ExactMatch:
                    destination.OldPath = UrlPath.Parse(source.OldPattern);
                    break;
                case RedirectRuleType.Regex:
                    destination.OldPattern = source.OldPattern;
                    break;
                case RedirectRuleType.Wildcard:
                    break;
            }
            
            return destination;
        }
    }
}