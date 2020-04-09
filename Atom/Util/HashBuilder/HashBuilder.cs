using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Atom.Util
{
    /// <summary>
    /// Builds MD5 has for a combination of built-in .NET types.
    /// </summary>
    public class HashBuilder
    {
        private readonly List<object> _items;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public HashBuilder()
        {
            _items = new List<object>();
        }

        /// <summary>
        /// Adds an item to a collection to be hashed.
        /// </summary>
        public HashBuilder Add<T>(T item)
        {
            if (CheckType.IsNullable<T>())
            {
                // add extra boolean for nullable types
                if (item != null)
                {
                    _items.Add(true);
                    _items.Add(item);
                }
                else
                {
                    _items.Add(false);
                }
            }
            else
            {
                _items.Add(item);
            }

            return this;
        }

        /// <summary>
        /// Adds multiple items to a collection to be hashed.
        /// </summary>
        public HashBuilder AddRange<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }

            return this;
        }

        /// <summary>
        /// Builds MD5 hash over the underlying collection.
        /// </summary>
        public Guid GetHash()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    foreach (var item in _items)
                    {
                        WriteItem(bw, item);
                    }

                    bw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var md5 = MD5.Create())
                    {
                        var bytes = md5.ComputeHash(ms);
                        return new Guid(bytes);
                    }
                }
            }
        }

        /// <summary>
        /// Writes item of a known type to underlying binary array, before calculating a hash.
        /// </summary>
        private static void WriteItem(BinaryWriter writer, object item)
        {
            switch (item)
            {
                case string value: writer.Write(value); break;
                case bool value: writer.Write(value); break;
                case byte value: writer.Write(value); break;
                case short value: writer.Write(value); break;
                case int value: writer.Write(value); break;
                case long value: writer.Write(value); break;
                case float value: writer.Write(value); break;
                case double value: writer.Write(value); break;
                case decimal value: writer.Write(value); break;

                case TimeSpan value:
                    writer.Write(value.Ticks);
                    break;

                case DateTime value:
                    writer.Write(value.Ticks);
                    break;

                case Guid value:
                    writer.Write(value.ToByteArray());
                    break;

                case byte[] value:
                    writer.Write(value);
                    break;

                default:
                    throw new InvalidOperationException($"Hashing is not supported for type '{item.GetType().Name}'.");
            }
        }
    }
}
