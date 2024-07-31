using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Parses comma separated text to and from list of strings.
    /// </summary>
    public static class CommaList
    {
        /// <summary>
        /// Parses comma separated string to a list of strings.
        /// </summary>
        public static List<string> AsCommaList(this string? value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return new List<string>();

            return value.Split(',')
                .Select(i => i.Trim())
                .Where(i => !String.IsNullOrWhiteSpace(i))
                .ToList();
        }

        /// <summary>
        /// Converts list of string to comma separated string.
        /// </summary>
        public static string? ToCommaList(this IEnumerable<string>? items)
        {
            if (items == null)
                return null;

            return String.Join(", ", items);
        }
    }
}
