using EPiServer.Data;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.DynamicData.Internal;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicData;

/// <summary>
/// Serves as a wrapper on the EPiServer.Data.Dynamic.DynamicDataStore class for testability purpose
/// </summary>
/// <typeparam name="T"></typeparam>
public class DynamicDataStore<T> : IDynamicDataStore<T>
    where T : class, IDynamicData
{
    private readonly ExtendedDynamicDataStoreFactory _dataStoreFactory;

    public DynamicDataStore(ExtendedDynamicDataStoreFactory dataStoreFactory)
    {
        _dataStoreFactory = dataStoreFactory;
    }

    public IQueryable<T> Items()
    {
        return CallActionOnDisposableStore(store => store.Items<T>());
    }

    public T GetById(Guid id)
    {
        return CallActionOnDisposableStore(store => store.Items<T>()
            .FirstOrDefault(o => o.Id.ExternalId == id));
    }

    public Identity Save(T item)
    {
        return CallActionOnDisposableStore(store => store.Save(item));
    }

    public void Delete(Guid id)
    {
        CallActionOnDisposableStore(store => store.Delete(Identity.NewIdentity(id)));
    }

    public void DeleteAll()
    {
        CallActionOnDisposableStore(s => s.DeleteAll());
    }

    private void CallActionOnDisposableStore(Action<DynamicDataStore> storeAction)
    {
        using var store = _dataStoreFactory.GetStore<T>();

        storeAction(store);
    }

    private TReturnType CallActionOnDisposableStore<TReturnType>(Func<DynamicDataStore, TReturnType> storeFunc)
    {
        using var store = _dataStoreFactory.GetStore<T>();

        return storeFunc(store);
    }
}
