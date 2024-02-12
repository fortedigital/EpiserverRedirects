using Forte.EpiserverRedirects.Model.RedirectRule;


namespace Forte.EpiserverRedirects.Repository
{
    public static class RedirectRuleStoreMapper
    {
        public static void MapForCreate(IRedirectRule from, IRedirectRule to)
        {
            MapForUpdate(from, to);
            to.RedirectOrigin = from.RedirectOrigin;
            to.CreatedOn = from.CreatedOn;
            to.CreatedBy = from.CreatedBy;
        }

        public static void MapForUpdate(IRedirectRule from, IRedirectRule to)
        {
            to.ContentId = from.ContentId;
            to.ContentProviderKey = from.ContentProviderKey;
            to.OldPattern = from.OldPattern;
            to.NewPattern = from.NewPattern;
            to.RedirectType = from.RedirectType;
            to.RedirectRuleType = from.RedirectRuleType;
            to.IsActive = from.IsActive;
            to.Notes = from.Notes;
            to.Priority = from.Priority;
            to.HostId = from.HostId;
        }
    }
}
