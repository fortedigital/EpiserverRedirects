using System;
using System.Collections.Generic;

namespace Forte.RedirectMiddleware.Model
{
    public class UrlPath : IEquatable<UrlPath>
    {
        public string NormalizedPath { get; }

        public UrlPath(string path)
        {
            NormalizedPath = NormalizePath(path);
        }

        private static string NormalizePath(string path)
        {
            path = path.Trim();
            
            path = path[0] == '/'
                ? path
                : '/' + path;

            if (path.Length > 1)
                path = path.TrimEnd('/');

            return path;
        }

        public static bool operator ==(UrlPath a, UrlPath b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(UrlPath a, UrlPath b)
        {
            return !(a == b);
        }

        public bool Equals(UrlPath other)
        {
            if (ReferenceEquals(null, other)) return false;

            if (ReferenceEquals(this, other)) return true;

            return NormalizedPath == other.NormalizedPath;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UrlPath) obj);
        }
        
        public override int GetHashCode()
        {
            return (NormalizedPath != null ? NormalizedPath.GetHashCode() : 0);
        }
    }
}