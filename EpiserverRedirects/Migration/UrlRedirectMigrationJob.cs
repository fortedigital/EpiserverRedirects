using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Forte.EpiserverRedirects.UrlRewritePlugin;

namespace Forte.EpiserverRedirects.Migration
{
    [ScheduledPlugIn(
        DisplayName = "UrlRedirectMigrationJob",
        Description = "A job for migration of System and Manual UrlRewrites to new RedirectRule Model")]
    public class UrlRedirectMigrationJob : ScheduledJobBase
    {
        private class MigrationResult
        {
            public MigrationResult(Identity id)
            {
                Id = id;
            }
            public Identity Id { get; }
            public string ErrorMessage { get; set; }
        }

        public override string Execute()
        {
            var redirectRulesRepository = ServiceLocator.Current.GetInstance<IRedirectRuleRepository>();
            
            var urlRewriteStore = DynamicDataStoreFactory.Instance.CreateStore(typeof(UrlRewriteModel));

            var urlRewrites = urlRewriteStore
                .Items<UrlRewriteModel>()
                .Where(ur => ur.Type == "System" || ur.Type == "Manual");

            var succeeded = new List<MigrationResult>();
            var failed = new List<MigrationResult>();

            foreach (var urlRewrite in urlRewrites)
            {
                var migrationResult = MigrateSingleRedirectRule(urlRewrite, redirectRulesRepository);

                if (migrationResult.ErrorMessage == null)
                    succeeded.Add(migrationResult);
                else
                    failed.Add(migrationResult);
            }

            return PrintShortMigrationResults(succeeded, failed);
        }

        private static MigrationResult MigrateSingleRedirectRule(UrlRewriteModel urlRewriteModel, IRedirectRuleRepository redirectRuleRepository)
        {
            var migrationResult = new MigrationResult(urlRewriteModel.Id);

            try { 
                var newRedirectRule = MapUrlRewriteToRedirectRule(urlRewriteModel);          
                redirectRuleRepository.Add(newRedirectRule);
            }
            catch (Exception e)
            {
                migrationResult.ErrorMessage = e.Message;
            }

            return migrationResult;
        }
        
        private static RedirectRule MapUrlRewriteToRedirectRule(UrlRewriteModel urlRewriteModel)
        {
            var redirectRule = new RedirectRule
            {
                OldPattern   = UrlPath.NormalizePath(urlRewriteModel.OldUrl),
                NewPattern = urlRewriteModel.NewUrl,
                RedirectType = MapStatusCodeToRedirectType(urlRewriteModel.RedirectStatusCode),
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                IsActive = IsMigratedRedirectRuleActive(urlRewriteModel.ContentId),
                RedirectOrigin = urlRewriteModel.ContentId != 0 ? RedirectOrigin.System : RedirectOrigin.Import,
                Notes = "Migrated from old redirects module",
                RedirectRuleType = MapUrlRewriteTypeToRedirectRuleType(urlRewriteModel.Type)
            };

            return redirectRule;
        }
        
        private static RedirectRuleType MapUrlRewriteTypeToRedirectRuleType(string urlRewriteModelType)
        {
            if (!Enum.TryParse(urlRewriteModelType, out UrlRedirectsType urlRedirectsType))
                return RedirectRuleType.ExactMatch;
            
            switch (urlRedirectsType)
            {
                case UrlRedirectsType.System:
                    return RedirectRuleType.ExactMatch;
                case UrlRedirectsType.Manual:
                    return RedirectRuleType.ExactMatch;
                case UrlRedirectsType.ManualWildcard:
                    return RedirectRuleType.Regex;
                default:
                    return RedirectRuleType.ExactMatch;
            }
        }

        private static bool IsMigratedRedirectRuleActive(int contentId)
        {
            if(contentId == 0)
                return false;

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var isContentDeleted = contentLoader.TryGet<IContent>(new ContentReference(contentId), out var content) == false ||
                   contentLoader.GetAncestors(content.ContentLink)
                       .Any(ancestor => ancestor.ContentLink == ContentReference.WasteBasket);

            return !isContentDeleted;
        }

        private static RedirectType MapStatusCodeToRedirectType(int statusCode)
        {
            switch (statusCode)
            {
                case 301:
                case 308:
                    return RedirectType.Permanent;
                
                case 302:
                case 307:
                    return RedirectType.Temporary;
                         
                default:
                    return RedirectType.Permanent;             
            }
        }

        private static string PrintShortMigrationResults(ICollection<MigrationResult> succeeded,
            ICollection<MigrationResult> failed)
        {
            return $"Succeeded: {succeeded.Count}, failed: {failed.Count}";
        }
        
        private static string PrintFullMigrationResults(ICollection<MigrationResult> succeeded,
            ICollection<MigrationResult> failed)
        {
            const string newLineSeparator = "\r\n";
            var migrationResults = new StringBuilder();
            migrationResults.Append($"Succeeded to migrate {succeeded.Count} redirectRules:{newLineSeparator}");
            migrationResults.Append(string.Join(newLineSeparator, succeeded.Select(r=>r.Id)));

            migrationResults.Append(newLineSeparator);
            
            migrationResults.Append($"Failed to migrate {failed.Count} redirectRules:{newLineSeparator}");
            migrationResults.Append(string.Join(newLineSeparator, failed.Select(r=>$"{r.Id}  {r.ErrorMessage}")));

            return migrationResults.ToString();
        }

    }
}