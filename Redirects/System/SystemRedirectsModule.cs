using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Forte.Redirects.Configuration;

namespace Forte.Redirects.System
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class SystemRedirectsModule : IInitializableModule
    {
        private const string OldUrlKey = "OLD_URL";

        private readonly UrlResolver _urlResolver;
        private readonly IContentVersionRepository _contentVersionRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILanguageBranchRepository _languageBranchRepository;

        public SystemRedirectsModule()
        {
            _urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            _contentVersionRepository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _languageBranchRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();
        }
        
        public SystemRedirectsModule(UrlResolver urlResolver, IContentVersionRepository contentVersionRepository,
            IContentRepository contentRepository, ILanguageBranchRepository languageBranchRepository)
        {
            _urlResolver = urlResolver;
            _contentVersionRepository = contentVersionRepository;
            _contentRepository = contentRepository;
            _languageBranchRepository = languageBranchRepository;
        }

        public void Initialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent += MovedContentHandler;
            events.PublishedContent += PublishedContentHandler;
            events.SavingContent += SavingContentHandler;
            events.SavedContent += SavedContentHandler;
            events.DeletedContent += DeletedContentHandler;
        }

        private void PublishedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;

            var lastVersion = _contentVersionRepository
                .List(e.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null)
                return;

            var oldUrl = GetContentUrl(lastVersion.ContentLink, lastVersion.LanguageBranch);
            
            if (oldUrl == null)
                return;

            var newUrl = GetContentUrl(e.ContentLink, e.Content.LanguageBranch());
            if (oldUrl == newUrl)
                return;

            var lastVersionPageData = _contentRepository.Get<IContentData>(lastVersion.ContentLink) as PageData;
            if (lastVersionPageData == null)
                return;

            SystemRedirects.AddRedirects(lastVersionPageData, oldUrl, GetCultureInfo(e));
        }

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments {ValidateTemplate = validateTemplate};
            return _urlResolver.GetVirtualPath(contentReference, language, arguments)?.VirtualPath;
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= MovedContentHandler;
            events.PublishedContent -= PublishedContentHandler;
            events.SavingContent -= SavingContentHandler;
            events.SavedContent -= SavedContentHandler;
            events.DeletedContent -= DeletedContentHandler;
        }

        private void MovedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;
            
            if (!(e.Content is IChangeTrackable))
                return;

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;

            if (originalParent == ContentReference.WasteBasket)
            {
                // do not create when restoring, cause not need to do redirects from waste basket.
                // however, DO redirect when moving to waste basket, because restore may be to another place 
                return;
            }
            
            foreach (var language in _languageBranchRepository.ListEnabled())
            {
                if (!(_contentRepository.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData))
                    continue;

                var oldUrl = GetContentUrl(originalParent, language.Culture.Name, false);
                if (oldUrl == null)
                    continue;

                SystemRedirects.AddRedirects(pageData, oldUrl + pageData.URLSegment, language.Culture);
            }
        }

        private void SavingContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;
            
            var transition = (e as SaveContentEventArgs)?.Transition;
            if (transition.Value.CurrentStatus == VersionStatus.NotCreated) return;

            // create redirects only if page is unpublished
            // because child objects may have been already published so their URL changes
            if (_contentVersionRepository.List(e.ContentLink).Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished)) return;

            var oldUrl = _urlResolver.GetUrl(e.ContentLink, null);

            e.Items.Add(OldUrlKey, oldUrl);
        }

        private void SavedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;
            
            var oldUrl = e.Items[OldUrlKey]?.ToString();
            if (oldUrl == null)
                return;

            var newUrl = _urlResolver.GetUrl(e.ContentLink, null);

            if(newUrl != oldUrl)
            {
                var pageData = _contentRepository.Get<IContentData>(e.ContentLink) as PageData;
                SystemRedirects.AddRedirects(pageData, oldUrl, GetCultureInfo(e));
            }

            e.Items.Remove(OldUrlKey);
        }

        private static void DeletedContentHandler(object sender, ContentEventArgs e)
        {
            SystemRedirects.DeleteRedirects(e.ContentLink, ((DeleteContentEventArgs) e).DeletedDescendents);
        }

        private static CultureInfo GetCultureInfo(ContentEventArgs e)
        {
            var localizable = e.Content as ILocalizable;
            return localizable?.Language;
        }
    }
}