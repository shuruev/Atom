using System;
using System.Linq;

namespace Atom.Util
{
    /// <summary>
    /// Can generate user-friendly .NET type name and aware of C# keywords.
    /// </summary>
    public static class TypeName
    {
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

        /// <summary>
        /// Returns friendly name for any .NET type, considering nullable types, generics, arrays, etc.
        /// E.g. Dictionary&lt;string, List&lt;int&gt;&gt;, int[], bool?, MyEnum?[]
        /// </summary>
        public static string GetFriendlyName<T>() => GetFriendlyName(typeof(T));

        /// <summary>
        /// Returns friendly name for any .NET type, considering nullable types, generics, arrays, etc.
        /// E.g. Dictionary&lt;string, List&lt;int&gt;&gt;, int[], bool?, MyEnum?[]
        /// </summary>
        public static string GetFriendlyName(Type type)
        {
            var keyword = GetKeyword(type);
            if (keyword != null)
                return keyword;

            var nullable = Nullable.GetUnderlyingType(type);
            if (nullable != null)
                return nullable.Name + "?";

            if (type.IsArray)
                return GetFriendlyName(type.GetElementType()) + "[]";

            if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + String.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)) + ">";

            return type.Name;
        }
    }
}
