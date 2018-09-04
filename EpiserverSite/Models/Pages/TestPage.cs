using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace EpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "TestPage", 
        GUID = "acbf31d7-a266-49d1-a106-bd449e6107da",
        Description = "My test page",
        GroupName = "Test Group")]
    public class TestPage : SitePageData
    {
        public virtual int Number { get; set; }
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}