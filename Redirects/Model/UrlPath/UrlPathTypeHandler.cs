using System;
using EPiServer.Data.Dynamic;

namespace Forte.Redirects.Model.UrlPath
{
    public class UrlPathTypeHandler : ITypeHandler
    {
        public Type MapToDatabaseType(Type type)
        {
            return typeof(string);
        }

        public object ToDatabaseFormat(string propertyName, object propertyValue, Type ownerType)
        {
            return propertyValue.ToString();
        }

        public object FromDatabaseFormat(string propertyName, object propertyValue, Type targetType, Type ownerType)
        {
            return UrlPath.Parse(propertyValue.ToString());
        }
    }
}