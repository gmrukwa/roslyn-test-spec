using System.Collections.Generic;

namespace Speccer
{
    public static class CollectionsExtension
    {
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> collection)
        {
            while (true)
            {
                foreach (var item in collection)
                    yield return item;
            }
        }
    }
}
