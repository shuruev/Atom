using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Builds MD5 hash for a combination of standard .NET types.
    /// </summary>
    public class HashBuilder
    {
        private readonly List<byte> _data = new();

        /// <summary>
        /// Adds a value to a collection to be hashed.
        /// </summary>
        public HashBuilder Add(object? value)
        {
            const int maxDataLengthToStore = 24;

            var block = GetDataBlock(value);
            if (block.Length > maxDataLengthToStore)
            {
                block = MD5.HashData(block);
            }

            _data.AddRange(block);
            return this;
        }

        /// <summary>
        /// Adds multiple values to a collection to be hashed.
        /// </summary>
        public HashBuilder AddMany(params object?[] values)
        {
            foreach (var value in values)
            {
                Add(value);
            }

            return this;
        }

        /// <summary>
        /// Builds MD5 hash over the underlying collection.
        /// </summary>
        public Guid GetHash()
        {
            var data = _data.ToArray();
            var hash = MD5.HashData(data);
            return new Guid(hash);
        }

        /// <summary>
        /// Converts value of a known type to a binary data block.
        /// </summary>
        private static byte[] GetDataBlock(object? value)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            if (value == null)
            {
                WriteNull(bw);
            }
            else
            {
                var type = value.GetType();
                if (CheckType.IsEnumerableOf(type, out var itemType)
                    && type != typeof(string))
                {
                    var dataType = WriteType(bw, itemType);
                    WriteValues(bw, dataType, (IEnumerable)value);
                }
                else
                {
                    var dataType = WriteType(bw, type);
                    WriteValue(bw, dataType, value);
                }
            }

            return ms.ToArray();
        }

        /// <summary>
        /// Writes 1-byte type code that represents <c>null</c> value.
        /// </summary>
        private static void WriteNull(BinaryWriter writer) => writer.Write((byte)DataType.Null);

        /// <summary>
        /// Writes 1-byte type code that represents known data type.
        /// </summary>
        private static DataType WriteType(BinaryWriter writer, Type type)
        {
            var dataType = GetKnownDataType(type);
            writer.Write((byte)dataType);
            return dataType;
        }

        /// <summary>
        /// Writes 1-byte code representing a list of values, followed by 4-byte list length,
        /// 1-byte nullability marker (whether the list contains any <c>null</c> values),
        /// and then every list value (byte length may vary depending on the type).
        /// </summary>
        private static void WriteValues(BinaryWriter writer, DataType dataType, IEnumerable values)
        {
            writer.Write(true);

            var items = values.Cast<object?>().ToList();
            writer.Write(items.Count);

            var hasNulls = items.Any(i => i == null);
            writer.Write(hasNulls);

            foreach (var item in items)
            {
                WriteValueBinary(writer, dataType, hasNulls, item);
            }
        }

        /// <summary>
        /// Writes 1-byte code representing single value, and then the value itself (byte length may vary depending on the type).
        /// </summary>
        private static void WriteValue(BinaryWriter writer, DataType dataType, object value)
        {
            writer.Write(false);
            WriteValueBinary(writer, dataType, false, value);
        }

        /// <summary>
        /// Writes value of some known data type as a binary block (byte length may vary depending on the type).
        /// </summary>
        private static void WriteValueBinary(BinaryWriter writer, DataType dataType, bool indicateForNullValues, object? value)
        {
            if (indicateForNullValues)
            {
                if (value == null)
                {
                    writer.Write(true);
                    return;
                }

                writer.Write(false);
            }

            switch (dataType)
            {
                case DataType.Boolean: writer.Write((bool)value!); break;
                case DataType.Byte: writer.Write((byte)value!); break;
                case DataType.Int16: writer.Write((short)value!); break;
                case DataType.Int32: writer.Write((int)value!); break;
                case DataType.Int64: writer.Write((long)value!); break;
                case DataType.Single: writer.Write((float)value!); break;
                case DataType.Double: writer.Write((double)value!); break;
                case DataType.Decimal: writer.Write((decimal)value!); break;
                case DataType.String: writer.Write((string)value!); break;

                default:
                    throw new InvalidOperationException($"Unknown data type: {dataType}.");
            }
        }

        /// <summary>
        /// Writes value of a known type as a binary data block.
        /// </summary>
        /*private static byte[] WriteDataBlock(BinaryWriter writer, object? value)
        {
            switch (value)
            {
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
        }*/

        /// <summary>
        /// Gets internal enum for a known data type, or throws exception saying this data type is not supported.
        /// </summary>
        private static DataType GetKnownDataType(Type type)
        {
            var nullable = Nullable.GetUnderlyingType(type);
            if (nullable != null)
                type = nullable;

            if (type == typeof(bool)) return DataType.Boolean;
            if (type == typeof(byte)) return DataType.Byte;
            if (type == typeof(short)) return DataType.Int16;
            if (type == typeof(int)) return DataType.Int32;
            if (type == typeof(long)) return DataType.Int64;
            if (type == typeof(float)) return DataType.Single;
            if (type == typeof(double)) return DataType.Double;
            if (type == typeof(decimal)) return DataType.Decimal;
            if (type == typeof(string)) return DataType.String;

            throw new InvalidOperationException($"Hashing is not supported for type '{type.Name}', use standard .NET types or collections.");
        }

        /// <summary>
        /// Internal enum representing every known data type that can be used for hashing.
        /// </summary>
        private enum DataType : byte
        {
            Null = 0,
            Boolean = 1,
            Byte = 2,
            Int16 = 3,
            Int32 = 4,
            Int64 = 5,
            Single = 6,
            Double = 7,
            Decimal = 8,
            String = 9,
        }
    }
}
