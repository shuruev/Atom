using System;

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
        /// Returns registered C# keyword for specified type (e.g. 'string', 'int', 'bool?', 'object' etc.),
        /// or returns null if this type does not have a keyword (e.g. 'TimeSpan?', 'Dictionary', etc.)
        /// </summary>
        public static string GetKeyword<T>() => GetKeyword(typeof(T));

        /// <summary>
        /// Returns registered C# keyword for specified type (e.g. 'string', 'int', 'bool?', 'object' etc.),
        /// or returns null if this type does not have a keyword (e.g. 'TimeSpan?', 'Dictionary', etc.)
        /// </summary>
        public static string GetKeyword(Type type)
        {
            switch (type)
            {
                case Type _ when type == typeof(void): return "void";
                case Type _ when type == typeof(object): return "object";
                case Type _ when type == typeof(string): return "string";

                case Type _ when type == typeof(char): return "char";
                case Type _ when type == typeof(char?): return "char?";

                case Type _ when type == typeof(bool): return "bool";
                case Type _ when type == typeof(bool?): return "bool?";

                case Type _ when type == typeof(byte): return "byte";
                case Type _ when type == typeof(byte?): return "byte?";
                case Type _ when type == typeof(sbyte): return "sbyte";
                case Type _ when type == typeof(sbyte?): return "sbyte?";

                case Type _ when type == typeof(short): return "short";
                case Type _ when type == typeof(short?): return "short?";
                case Type _ when type == typeof(ushort): return "ushort";
                case Type _ when type == typeof(ushort?): return "ushort?";

                case Type _ when type == typeof(int): return "int";
                case Type _ when type == typeof(int?): return "int?";
                case Type _ when type == typeof(uint): return "uint";
                case Type _ when type == typeof(uint?): return "uint?";

                case Type _ when type == typeof(long): return "long";
                case Type _ when type == typeof(long?): return "long?";
                case Type _ when type == typeof(ulong): return "ulong";
                case Type _ when type == typeof(ulong?): return "ulong?";

                case Type _ when type == typeof(float): return "float";
                case Type _ when type == typeof(float?): return "float?";
                case Type _ when type == typeof(double): return "double";
                case Type _ when type == typeof(double?): return "double?";

                case Type _ when type == typeof(decimal): return "decimal";
                case Type _ when type == typeof(decimal?): return "decimal?";

                default: return null;
            }
        }
    }
}
