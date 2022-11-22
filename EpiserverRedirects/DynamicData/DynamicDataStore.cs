using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataStore<T> : IDynamicDataStore<T>
         where T : class, IDynamicData
    {
        private readonly EPiServer.Data.Dynamic.DynamicDataStore store;

        public DynamicDataStore(DynamicDataStoreFactory ddsFactory)
        {
            this.store = ddsFactory.CreateStore(typeof(T));
        }

        public IQueryable<T> Items()
        {
            return store.Items<T>();
        }

        public T GetById(Guid id)
        {
            return store.Items<T>()
                .FirstOrDefault(o => o.Id.ExternalId == id);
        }

        public Identity Save(T item)
        {
            return store.Save(item);
        }

        public void Delete(Guid id)
        {
            store.Delete(Identity.NewIdentity(id));
        }

        public void DeleteAll()
        {
            store.DeleteAll();
        }
    }
}
