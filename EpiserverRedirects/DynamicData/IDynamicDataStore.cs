using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Linq;

namespace Forte.EpiserverRedirects.DynamicData
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
}
