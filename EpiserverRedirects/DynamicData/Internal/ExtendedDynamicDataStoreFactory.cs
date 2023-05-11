using EPiServer.Data.Dynamic;
using EPiServer.Data.Dynamic.Providers;
using EPiServer.Shell.Composition;


namespace Forte.EpiserverRedirects.DynamicData.Internal
{
    /// <summary>
    /// Serves as a factory to create an instance of the ExtendedEPiServerDynamicDataStore
    /// </summary>
    public class ExtendedDynamicDataStoreFactory
    {
        private readonly DynamicDataStoreFactory _dataStoreFactory;
        private readonly IDataStoreProviderFactory _dataStoreProviderFactory;

        public ExtendedDynamicDataStoreFactory(DynamicDataStoreFactory dataStoreFactory, IDataStoreProviderFactory dataStoreProviderFactory)
        {
            _dataStoreFactory = dataStoreFactory;
            _dataStoreProviderFactory = dataStoreProviderFactory;
        }

        public DynamicDataStore GetOrCreateStore<T>()
            where T : class, IDynamicData
        {
            var store = _dataStoreFactory.GetOrCreateStore(typeof(T));
            return new ExtendedEPiServerDynamicDataStore<T>(store.StoreDefinition, _dataStoreProviderFactory.Create());
        }
    }
}
