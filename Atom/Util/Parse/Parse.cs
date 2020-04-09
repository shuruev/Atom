using System;
using System.ComponentModel;
using System.Globalization;

namespace Atom.Util
{
    /// <summary>
    /// Provides extensions method for parsing string into .NET built-in types.
    /// </summary>
    public static class Parse
    {
        /// <summary>
        /// Parses string value as specified type.
        /// </summary>
        public static T As<T>(this string value)
        {
            try
            {
                switch (typeof(T))
                {
                    case Type _ when typeof(T) == typeof(string):
                        return (T)(object)value;

                    case Type _ when typeof(T) == typeof(bool):
                    case Type _ when typeof(T) == typeof(bool?):
                    case Type _ when typeof(T) == typeof(byte):
                    case Type _ when typeof(T) == typeof(byte?):
                    case Type _ when typeof(T) == typeof(short):
                    case Type _ when typeof(T) == typeof(short?):
                    case Type _ when typeof(T) == typeof(int):
                    case Type _ when typeof(T) == typeof(int?):
                    case Type _ when typeof(T) == typeof(long):
                    case Type _ when typeof(T) == typeof(long?):
                    case Type _ when typeof(T) == typeof(float):
                    case Type _ when typeof(T) == typeof(float?):
                    case Type _ when typeof(T) == typeof(double):
                    case Type _ when typeof(T) == typeof(double?):
                    case Type _ when typeof(T) == typeof(decimal):
                    case Type _ when typeof(T) == typeof(decimal?):
                    case Type _ when typeof(T) == typeof(TimeSpan):
                    case Type _ when typeof(T) == typeof(TimeSpan?):
                    case Type _ when typeof(T) == typeof(DateTime):
                    case Type _ when typeof(T) == typeof(DateTime?):
                    case Type _ when typeof(T) == typeof(Guid):
                    case Type _ when typeof(T) == typeof(Guid?):
                    case Type _ when CheckType.IsEnum<T>():
                        return ParseFromString<T>(value);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Cannot parse {GetTypeName<T>()} value from string '{value}'.", e);
            }

            throw new InvalidOperationException($"Parsing as {GetTypeName<T>()} is not supported.");
        }

        /// <summary>
        /// Universal method which can parse the most commonly known types using .NET built-in converters.
        /// </summary>
        private static T ParseFromString<T>(string value)
        {
            var type = typeof(T);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                // special case which allows whitespace strings
                // to be used as nulls for nullable types
                if (String.IsNullOrWhiteSpace(value))
                    return (T)(object)null;
            }
            else
            {
                if (type.IsValueType)
                {
                    // special case which doesn't allow whitespace strings
                    // to be parsed as default values for some value types
                    // (i.e. built-in converter below would do that for DateTime)
                    if (String.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Cannot parse non-nullable value type from null or whitespace string.");
                }
            }

            var converter = TypeDescriptor.GetConverter(type);
            return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        }

        /// <summary>
        /// Gets type name for exception messages.
        /// </summary>
        private static string GetTypeName<T>()
        {
            var type = typeof(T);

            var nullable = Nullable.GetUnderlyingType(type);
            if (nullable != null)
            {
                return $"Nullable<{nullable.Name}>";
            }

            return type.Name;
        }
    }
}
