using EPiServer.Data.Dynamic;
using EPiServer.Data.Dynamic.Providers;


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

        public DynamicDataStore GetStore<T>()
            where T : class, IDynamicData
        {
            var storeDefinition = StoreDefinition.Get(_dataStoreFactory.GetStoreNameForType(typeof(T)));
            if (storeDefinition == null)
            {
                // if the store does not exist, create it with the EPiServer genuine StoreProviderFactory implementation
                _dataStoreFactory.CreateStore(typeof(T));
                storeDefinition = StoreDefinition.Get(_dataStoreFactory.GetStoreNameForType(typeof(T)));
            }

            return new ExtendedEPiServerDynamicDataStore<T>(storeDefinition, _dataStoreProviderFactory.Create());
        }
    }
}
