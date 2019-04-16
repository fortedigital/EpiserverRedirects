using System;
using System.Collections.Generic;

namespace Forte.RedirectMiddleware.Model
{
    public class UrlPath : IEquatable<UrlPath>
    {
        public string NormalizedPath { get; }

        public static UrlPath Create(string oldPath)
        {
            var trimmedOldPath = oldPath.Trim();
            ValidatePath(trimmedOldPath);
            var urlPath = new UrlPath(trimmedOldPath);
            return urlPath;
        }
        public static bool TryCreate(string oldPath, out UrlPath urlPath)
        {
            try
            {             
                urlPath = Create(oldPath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Entered path is not a valid relative path: " + e);
                urlPath = null;
                return false;
            }
        }
        private UrlPath(string oldPath)
        {
            NormalizedPath = NormalizePath(oldPath);
        }

        private static string NormalizePath(string path)
        {            
            path = path[0] == '/'
                ? path
                : '/' + path;

            if (path.Length > 1)
                path = path.TrimEnd('/');

            return path;
        }
        
        private static void ValidatePath(string oldPath)
        {
            new Uri(oldPath, UriKind.Relative);
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

            return string.Equals(NormalizedPath, other.NormalizedPath, StringComparison.CurrentCulture);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UrlPath) obj);
        }
        
        //Adjust to the same StringComparison type as Equals?
        public override int GetHashCode()
        {
            return (NormalizedPath != null ? NormalizedPath.GetHashCode() : 0);
        }
    }
}