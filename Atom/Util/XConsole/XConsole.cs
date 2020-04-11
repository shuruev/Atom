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
        public static XConsole Write(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => WriteColored(message, foreground, background);

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, using specified colors.
        /// </summary>
        public static XConsole WriteLine(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => WriteLineColored(message, foreground, background);

        /// <summary>
        /// Writes specified string message (or nothing) to the colored output.
        /// </summary>
        public XConsole Write(string message = null) => WriteColored(message, _foreground, _background);

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, to the colored output.
        /// </summary>
        public XConsole WriteLine(string message = null) => WriteLineColored(message, _foreground, _background);

        private static XConsole WriteLineColored(string message, ConsoleColor? foreground, ConsoleColor? background) => WriteColored(message + Environment.NewLine, foreground, background);

        private static XConsole WriteColored(string message, ConsoleColor? foreground, ConsoleColor? background)
        {
            var console = With(foreground, background);

            // do nothing if null is passed
            if (message == null)
                return console;

            // do not lock or save/restore colors, when not needed
            if (foreground == null && background == null)
            {
                Console.Write(message);
                return console;
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
                        WriteColoredRaw(chunk, foreground, background);
                }
            }

            return console;
        }

        private static void WriteColoredRaw(char[] symbols, ConsoleColor? foreground, ConsoleColor? background)
        {
            var currentForeground = Console.ForegroundColor;
            var currentBackground = Console.BackgroundColor;

            if (foreground != null)
                Console.ForegroundColor = foreground.Value;

            if (background != null)
                Console.BackgroundColor = background.Value;

            Console.Write(symbols);

            if (foreground != null)
                Console.ForegroundColor = currentForeground;

            if (background != null)
                Console.BackgroundColor = currentBackground;
        }
    }
}
