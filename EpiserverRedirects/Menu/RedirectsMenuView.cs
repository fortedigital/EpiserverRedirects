using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.ViewComposition.Containers;

namespace Forte.EpiserverRedirects.Menu
{
    [CompositeView]
    public class RedirectsMenuView : ICompositeView
    {
        private const string ViewName = "RedirectsMenu";
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

        public ICompositeView CreateView() => new RedirectsMenuView();
    }
}