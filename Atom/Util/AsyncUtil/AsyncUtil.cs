using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atom.Util
{
    /// <summary>
    /// Helper methods for async collections processing.
    /// </summary>
    public static class AsyncUtil
    {
        /// <summary>
        /// Processes a collection in an asynchronous way, with limiting maximum concurrent invocations.
        /// Note that collection will be fully rendered before processing.
        /// </summary>
        public static Task ForEachAsync<T>(IEnumerable<T> source, int maxParallel, Func<T, Task> body)
        {
            var items = new ConcurrentQueue<T>(source);
            return Task.WhenAll(
                Enumerable.Repeat(0, maxParallel)
                    .Select(_ => Task.Run(async () =>
                    {
                        while (items.TryDequeue(out var val))
                            await body(val).ConfigureAwait(false);
                    })));
        }

        /// <summary>
        /// Processes a collection in an asynchronous way, with limiting maximum concurrent invocations.
        /// This will not render the entire collection prior to processing, but instead will be rendering it as processing goes.
        /// As a result it works a little bit slower, but may come in handy for huge and/or slow collections.
        /// </summary>
        public static Task ForEachLazyAsync<T>(IEnumerable<T> source, int maxParallel, Func<T, Task> body)
        {
            return Task.WhenAll(
                Partitioner.Create(source)
                    .GetPartitions(maxParallel)
                    .Select(partition => Task.Run(async () =>
                    {
                        using (partition)
                        {
                            while (partition.MoveNext())
                                await body(partition.Current).ConfigureAwait(false);
                        }
                    })));
        }
    }
}
