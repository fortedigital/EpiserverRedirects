using System;
using System.Web;

namespace Forte.EpiserverRedirects.Model
{
    public class UrlPath : IEquatable<UrlPath>
    {
        private const string InvalidRelativePathExceptionMessage = "Entered path is not a valid relative path.";
        private const string InvalidUrlExceptionMessage = "Entered url is not valid.";
        private Uri Path { get; }
           
        public static UrlPath Parse(string oldPath)
        {
            try
            {
                var normalizedPath = NormalizePath(oldPath);
                var urlPath = new UrlPath(normalizedPath);
                return urlPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentException(InvalidRelativePathExceptionMessage, e);
            }
        }
        public static bool TryParse(string oldPath, out UrlPath urlPath)
        {
            try
            {             
                urlPath = Parse(oldPath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(InvalidRelativePathExceptionMessage + e);
                urlPath = null;
                return false;
            }
        }

        public static string ExtractRelativePath(string url)
        {
            try
            {
                var isAbsoluteUriParseOk = Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri);
                var path = isAbsoluteUriParseOk ? uri.LocalPath : url;
                var normalizedPath = NormalizePath(path);

                return normalizedPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentException(InvalidUrlExceptionMessage, e);
            }
        }
        
        public static UrlPath FromUri(Uri uri)
        {
            try
            {
                var localPath = uri.LocalPath;
                var normalizedPath = NormalizePath(localPath);
                var urlPath = new UrlPath(normalizedPath);

                return urlPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentException(InvalidRelativePathExceptionMessage, e);
            }
        }
        
        private UrlPath(string oldPath)
        {
            Path = new Uri(oldPath, UriKind.Relative);
        }

        public static string NormalizePath(string path)
        {
            path = path.Trim();
            path = path[0] == '/'
                ? path
                : '/' + path;

            if (path.Length > 1)
                path = path.TrimEnd('/');

            return path;
        }

        public static string EnsurePathEncoding(string path)
        {
            return path != null ? Uri.EscapeUriString(HttpUtility.UrlDecode(path)) : path;
        }

        public static bool operator ==(UrlPath a, UrlPath b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(UrlPath a, UrlPath b)
        {
            return !(a == b);
        }

        private bool Equals(UrlPath other, StringComparison stringComparison)
        {
            if (ReferenceEquals(null, other)) return false;

            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Path.OriginalString, other.Path.OriginalString, stringComparison);
        }

        public bool Equals(UrlPath other)
        {
            if (ReferenceEquals(null, other)) return false;

            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Path.OriginalString, other.Path.OriginalString, StringComparison.Ordinal);
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
            return (Path?.OriginalString != null ? Path.OriginalString.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Path.OriginalString;
        }
    }
}