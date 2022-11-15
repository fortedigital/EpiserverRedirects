using EPiServer.Data;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.DynamicData.Internal;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataStore<T> : IDynamicDataStore<T>
         where T : class, IDynamicData
    {
        private readonly DynamicDataStore _store;

        public DynamicDataStore(ExtendedDynamicDataStoreFactory dataStoreFactory)
        {
            _store = dataStoreFactory.GetStore<T>();
        }

        public IQueryable<T> Items()
        {
            return _store.Items<T>();
        }

        public T GetById(Guid id)
        {
            return _store.Items<T>()
                .FirstOrDefault(o => o.Id.ExternalId == id);
        }

        public Identity Save(T item)
        {
            return _store.Save(item);
        }

        public void Delete(Guid id)
        {
            _store.Delete(Identity.NewIdentity(id));
        }

        public void DeleteAll()
        {
            _store.DeleteAll();
        }
    }
}
