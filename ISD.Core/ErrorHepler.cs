using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.Core
{
    public static class ErrorHepler
    {

        // all error checking left out for brevity

        // a.k.a., linked list style enumerator
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
        public static List<string> GetaAllMessages(this Exception exception)
        {
            return exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message).ToList();
        }

    }
}
