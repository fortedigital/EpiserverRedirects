using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Data.Dynamic.Internal;
using EPiServer.Data.Dynamic.Providers;


namespace Forte.EpiserverRedirects.DynamicData.Internal
{
    /// <summary>
    /// This class overrides the default behavior of the EPiServerDynamicDataStore while extracting records from the sql server store
    /// Default behavior deserializes extracted data based on the generic class with which the store.Items<T>() method was called
    /// Overriden behavior uses strictly the same class with wich the Store was created. If you try to extract data using some other type, it will fail with type cast exception.
    /// </summary>
    /// <typeparam name="TEntityType"></typeparam>
    internal class ExtendedEPiServerDynamicDataStore<TEntityType> : EPiServerDynamicDataStore
        where TEntityType : class, IDynamicData
    {
        public ExtendedEPiServerDynamicDataStore(StoreDefinition storeDefinition, DataStoreProvider dataStoreProvider) : base(storeDefinition, dataStoreProvider)
        {
        }

        public override TResult Load<TResult>(Identity id) => (TResult)(object)base.Load<TEntityType>(id);
    }
}
