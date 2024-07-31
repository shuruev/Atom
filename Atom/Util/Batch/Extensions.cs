using System.Collections.Generic;
using System.Linq;

namespace Atom.Util
{
    /// <summary>
    /// Extension methods for splitting collection into batches.
    /// </summary>
    public static class BatchExtension
    {
        /// <summary>
        /// Splits specified collection into batches of fixed size.
        /// The last batch may have less items than all other batches.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int size)
        {
            var i = 0;
            return items.GroupBy(x => i++ / size);
        }
    }
}
