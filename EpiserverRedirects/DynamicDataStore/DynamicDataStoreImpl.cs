using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicDataStore
{
    public interface IDynamicDataStore<T>
        where T : IDynamicData
    {
        public IQueryable<T> Items();
        public T GetById(Guid id);
        public Identity Save(T item);
        public void Delete(Guid id);
        public void DeleteAll();
    }

    public class DynamicDataStoreImpl<T> : IDynamicDataStore<T>
         where T : class, IDynamicData
    {
        private readonly EPiServer.Data.Dynamic.DynamicDataStore store;

        public DynamicDataStoreImpl(DynamicDataStoreFactory ddsFactory)
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
