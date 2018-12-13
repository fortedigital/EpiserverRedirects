using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.ViewComposition.Containers;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Menu
{
    [CompositeView]
    public class UrlRedirectsMenuView : ICompositeView
    {
        private const string ViewName = "UrlRedirectsMenu";
        private IContainer _rootContainer;

        public string Name
        {
            get { return ViewName; }
        }

        public string Title
        {
            get { return ViewName; }
        }

        public string DefaultContext
        {
            get { return null; }
        }

        public IContainer RootContainer
        {
            get
            {
                if (_rootContainer == null)
                {
                    var customContainer = new CustomContainer("urlRedirectsMenu/RootContainer");
                    customContainer.Settings.Add("id", Name + "_rootContainer");
                    customContainer.Settings.Add("persist", "true"); 
                    _rootContainer = customContainer;
                }
                return _rootContainer;
            }
        }

        public ICompositeView CreateView()
        {
            return new UrlRedirectsMenuView();
        }
    }
}