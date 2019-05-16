using EPiServer.Data.Dynamic;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Forte.Redirects.Model.UrlPath
{
    [InitializableModule]
    public class RegisterTypeHandlerModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if(!GlobalTypeHandlers.Instance.ContainsKey(typeof(UrlPath)))
                GlobalTypeHandlers.Instance.Add(typeof(UrlPath), new UrlPathTypeHandler());        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}