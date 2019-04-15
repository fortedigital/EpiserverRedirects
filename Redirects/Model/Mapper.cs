namespace Forte.RedirectMiddleware.Model
{
    public static class RedirectRuleMapper
    {
        public static void ModelToDto(RedirectRule source, RedirectRuleDto destination)
        {
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = source.OldPath.NormalizedPath;
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
        }
        
        public static RedirectRuleDto ModelToDto(RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            ModelToDto(source, destination);
            return destination;
        }
        
        public static void DtoToModel(RedirectRuleDto source, RedirectRule destination)
        {
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = new UrlPath(source.OldPath);
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
        }
        
        public static RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = new RedirectRule();
            DtoToModel(source, destination);
            return destination;
        }
    }
}