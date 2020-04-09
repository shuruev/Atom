using System;

namespace Atom.Util
{
    /// <summary>
    /// Checks multiple criteria for built-in .NET types (is nullable, is enum, etc.)
    /// </summary>
    public static class CheckType
    {
        /// <summary>
        /// Checks if generic type is nullable, i.e. can hold null value.
        /// </summary>
        public static bool IsNullable<T>()
        {
            var type = typeof(T);

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
        /// Checks if generic type is enum, or nullable enum.
        /// </summary>
        public static bool IsEnum<T>()
        {
            var type = typeof(T);

            // nullable enum
            var nullable = Nullable.GetUnderlyingType(type);
            if (nullable != null)
            {
                return nullable.IsEnum;
            }

            // regular enum
            return type.IsEnum;
        }
    }
}
