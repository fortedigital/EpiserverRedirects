using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using AlloyDemoKit.Models.Pages;
using PowerSlice;

namespace AlloyDemoKit.Business.PowerSlice
{

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class ArticleSlice : ContentSliceBase<ArticlePage>
    {
        public override string Name
        {
            get { return "Articles"; }
        }

        public override string DisplayName
        {
            get { return "Articles"; }
        }

        public override int Order
        {
            get { return 4; }
        }

        public override bool HideSortOptions
        {
            get
            {
                return true;
            }
        }
    }
}