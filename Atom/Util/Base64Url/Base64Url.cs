using System;

namespace Atom.Util
{
    /// <summary>
    /// Implements Base-64 encoding with URL and filename safe alphabet (https://tools.ietf.org/html/rfc4648#section-5).
    /// </summary>
    public static class Base64Url
    {
        /// <summary>
        /// Encodes byte array to Base-64 string with with URL and filename safe alphabet.
        /// </summary>
        public static string Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var base64 = Convert.ToBase64String(data);
            return base64
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <summary>
        /// Decodes Base-64 string to byte array. Will work with both URL and filename safe alphabet, or regular Base-64 string.
        /// </summary>
        public static byte[] Decode(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var base64 = text
                .Replace('-', '+')
                .Replace('_', '/');

            switch (text.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
                default:
                    throw new ArgumentException("Invalid Base-64 string length.");
            }

            return Convert.FromBase64String(base64);
        }
    }
}
