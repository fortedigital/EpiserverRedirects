using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Forte.EpiserverRedirects.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source )
    {
        return source == null || source.GetEnumerator().MoveNext() == false;
    }
}
