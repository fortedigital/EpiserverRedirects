using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Mapper
{
    public class RedirectRuleMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleMapper() : base(ModelToDtoDelegate, DtoToModelDelegate)
        {
        }
        

        private static RedirectRuleDto ModelToDtoDelegate(Model.RedirectRule.RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            destination.Id = source.Id;
            destination.NewUrl = source.NewPattern;
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;
            
            destination.RedirectRuleType = source.RedirectRuleType;

            switch (destination.RedirectRuleType)
            {
                case RedirectRuleType.ExactMatch:
                    destination.Pattern = source.OldPath.Path.OriginalString;
                    break;
                case RedirectRuleType.Regex:
                    destination.Pattern = source.OldPattern;
                    break;
                case RedirectRuleType.Wildcard:
                    break;
            }

            return destination;
        }

        public static Model.RedirectRule.RedirectRule DtoToModelDelegate(RedirectRuleDto source)
        {
            var destination = new Model.RedirectRule.RedirectRule();
            destination.Id = source.Id;
            destination.NewPattern = source.NewUrl;

            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;
            destination.RedirectRuleType = source.RedirectRuleType;

            switch (destination.RedirectRuleType)
            {
                case RedirectRuleType.ExactMatch:
                    destination.OldPath = Model.UrlPath.UrlPath.Parse(source.Pattern);
                    break;
                case RedirectRuleType.Regex:
                    destination.OldPattern = source.Pattern;
                    break;
                case RedirectRuleType.Wildcard:
                    break;
            }
            
            return destination;
        }
    }
}