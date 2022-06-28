using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Checks multiple criteria for built-in .NET types (is nullable, is enum, etc.)
    /// </summary>
    public static class CheckType
    {
        /// <summary>
        /// Checks if specified type is nullable, i.e. can hold null value.
        /// </summary>
        public static bool IsNullable<T>() => IsNullable(typeof(T));

        /// <summary>
        /// Checks if specified type is nullable, i.e. can hold null value.
        /// </summary>
        public static bool IsNullable(Type type)
        {
            // reference type
            if (!type.IsValueType)
                return true;

            // Nullable<T>
            if (Nullable.GetUnderlyingType(type) != null)
                return true;

            // value type
            return false;
        }

        /// <summary>
        /// Checks if specified type is enum, or nullable enum.
        /// </summary>
        public static bool IsEnum<T>() => IsEnum(typeof(T));

        /// <summary>
        /// Checks if specified type is enum, or nullable enum.
        /// </summary>
        public static bool IsEnum(Type type)
        {
            // nullable enum
            var nullable = Nullable.GetUnderlyingType(type);
            if (nullable != null)
            {
                return nullable.IsEnum;
            }

            // regular enum
            return type.IsEnum;
        }

        /// <summary>
        /// Checks if specified type is enumerable over some specific type,
        /// i.e. inherits <see cref="IEnumerable{T}"/>.
        /// Also returns the underlying item type.
        /// </summary>
        public static bool IsEnumerableOf<T>(out Type itemType) => IsEnumerableOf(typeof(T), out itemType);

        /// <summary>
        /// Checks if specified type is enumerable over some specific type,
        /// i.e. inherits <see cref="IEnumerable{T}"/>.
        /// Also returns the underlying item type.
        /// </summary>
        public static bool IsEnumerableOf(Type type, out Type itemType)
        {
            var checkInterface = type.GetInterfaces().Append(type)
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (checkInterface != null)
            {
                itemType = checkInterface.GetGenericArguments()[0];
                if (!itemType.IsGenericParameter)
                    return true;
            }

            itemType = typeof(void);
            return false;
        }
    }
}
