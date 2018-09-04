using EpiserverSite.Business.Rendering;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EpiserverSite.Models.Pages
{
    /// <summary>
    /// Used to logically group pages in the page tree
    /// </summary>
    [SiteContentType(
        GUID = "D178950C-D20E-4A46-90BD-5338B2424745",
        GroupName = Global.GroupNames.Specialized)]
    [SiteImageUrl]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(ContainerPage), typeof(ProductPage), typeof(StandardPage), typeof(ISearchPage), typeof(LandingPage), typeof(TestPage) })]
    public class ContainerPage : SitePageData, IContainerPage
    {
        
    }
}
