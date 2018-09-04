using EPiServer.Core;
using EpiserverSite.Models.Pages;

namespace EpiserverSite.Models.ViewModels
{
    public class TestViewModel : PageViewModel<TestPage>
    {
        public int Number { get; set; }
        public string ImageUrl { get; set; }

        public TestViewModel(TestPage currentPage) 
            : base(currentPage)
        {
        }
    }
}