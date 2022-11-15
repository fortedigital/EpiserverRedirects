using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicData
{
    /// <summary>
    /// Represents interface for a wrapper class on the EPiServer.Data.Dynamic.DynamicDataStore
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
