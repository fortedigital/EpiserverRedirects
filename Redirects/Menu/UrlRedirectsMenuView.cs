using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.ViewComposition.Containers;

namespace Forte.Redirects.Menu
{
    [CompositeView]
    public class UrlRedirectsMenuView : ICompositeView
    {
        private const string ViewName = "UrlRedirectsMenu";
        private IContainer _rootContainer;

        public string Name => ViewName;

        public string Title => ViewName;

        public string DefaultContext => null;

        public IContainer RootContainer
        {
            get
            {
                if (_rootContainer == null)
                {
                    var customContainer = new CustomContainer("redirectsMenu/RootContainer");
                    customContainer.Settings.Add("id", Name + "_rootContainer");
                    customContainer.Settings.Add("persist", "true"); 
                    _rootContainer = customContainer;
                }
                return _rootContainer;
            }
        }

        public ICompositeView CreateView() => new UrlRedirectsMenuView();
    }
}