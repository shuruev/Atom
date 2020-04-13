using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Atom.Util
{
    /// <summary>
    /// Colored console writer.
    /// </summary>
    [SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
    public partial class XConsole
    {
        private static readonly object _sync = new object();

        private readonly ConsoleColor? _foreground;
        private readonly ConsoleColor? _background;

        /// <summary>
        /// Initializes new instance.
        /// </summary>
        private XConsole(ConsoleColor? foreground, ConsoleColor? background)
        {
            _foreground = foreground;
            _background = background;
        }

        /// <summary>
        /// Creates new console writer with specified colors.
        /// </summary>
        public static XConsole With(ConsoleColor? foreground = null, ConsoleColor? background = null) => new XConsole(foreground, background);

        /// <summary>
        /// Switches to new console writer with specified colors.
        /// </summary>
        public XConsole To(ConsoleColor? foreground = null, ConsoleColor? background = null) => With(foreground, background);

        /// <summary>
        /// Writes specified string message (or nothing) using specified colors.
        /// </summary>
        public static XConsole Write(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => With(foreground, background).Write(message);

        /// <summary>
        /// Writes specified character using specified colors.
        /// </summary>
        public static XConsole Write(char c, ConsoleColor? foreground = null, ConsoleColor? background = null) => With(foreground, background).Write(c);

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, using specified colors.
        /// </summary>
        public static XConsole WriteLine(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => With(foreground, background).WriteLine(message);

        /// <summary>
        /// Writes specified string message (or nothing) to the colored output.
        /// </summary>
        public XConsole Write(string message = null) => WriteColored(message);

        /// <summary>
        /// Writes specified character to the colored output.
        /// </summary>
        public XConsole Write(char c) => WriteColored(ToString(c));

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, to the colored output.
        /// </summary>
        public XConsole WriteLine(string message = null) => WriteColored(ToLine(message));

        private static string ToString(char c) => Char.ToString(c);
        private static string ToLine(string message) => message + Environment.NewLine;

        private bool IsCurrent => _foreground == null && _background == null;

        private XConsole WriteColored(string message)
        {
            // do nothing if null is passed
            if (message == null)
                return this;

            // do not lock or save/restore colors, when not needed
            if (IsCurrent)
            {
                Console.Write(message);
                return this;
            }

            // split into chunks with control and non-control characters
            var chunks = new List<char[]>();
            var current = new List<char>();
            bool? control = null;
            foreach (var c in message)
            {
                if (Char.IsControl(c) != control)
                {
                    if (current.Count > 0)
                        chunks.Add(current.ToArray());

                    current.Clear();
                    control = Char.IsControl(c);
                }

                current.Add(c);
            }

            if (current.Count > 0)
                chunks.Add(current.ToArray());

            // use lock to avoid color overrides from multiple thread
            lock (_sync)
            {
                foreach (var chunk in chunks)
                {
                    if (Char.IsControl(chunk[0]))
                        Console.Write(chunk);
                    else
                        WriteColoredRaw(chunk);
                }
            }

            return this;
        }

        private void WriteColoredRaw(char[] symbols)
        {
            var currentForeground = Console.ForegroundColor;
            var currentBackground = Console.BackgroundColor;

            if (_foreground != null)
                Console.ForegroundColor = _foreground.Value;

            if (_background != null)
                Console.BackgroundColor = _background.Value;

            Console.Write(symbols);

            if (_foreground != null)
                Console.ForegroundColor = currentForeground;

            if (_background != null)
                Console.BackgroundColor = currentBackground;
        }
    }
}
