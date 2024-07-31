using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Atom.Util
{
    /// <summary>
    /// Extends standard console providing more features like colored output.
    /// </summary>
    public static partial class XConsole
    {
        /// <summary>
        /// Public object which can be used to lock during console output from multiple threads.
        /// </summary>
        public static readonly object Sync = new object();

        /// <summary>
        /// Creates new console writer with specified colors.
        /// </summary>
        public static XConsoleImpl To(ConsoleColor? foreground = null, ConsoleColor? background = null) => new XConsoleImpl(foreground, background);

        /// <summary>
        /// Writes specified string message (or nothing) using specified colors.
        /// </summary>
        public static XConsoleImpl Write(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => To(foreground, background).Write(message);

        /// <summary>
        /// Writes specified character using specified colors.
        /// </summary>
        public static XConsoleImpl Write(char c, ConsoleColor? foreground = null, ConsoleColor? background = null) => To(foreground, background).Write(c);

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, using specified colors.
        /// </summary>
        public static XConsoleImpl WriteLine(string message = null, ConsoleColor? foreground = null, ConsoleColor? background = null) => To(foreground, background).WriteLine(message);

        /// <summary>
        /// Makes sure the current console position is at the line beginning.
        /// If it is already, does nothing. If it is not, writes line terminator and proceeds to the new line.
        /// </summary>
        public static XConsoleImpl NewLine() => Default.NewLine();

        /// <summary>
        /// Adds one blank line and starts from new line.
        /// If current line is not terminated, it will be terminated prior to rendering the blank line.
        /// </summary>
        public static XConsoleImpl NewPara() => Default.NewPara();

        /// <summary>
        /// Writes "Press any key..." on a new paragraph, then awaits until any key is pressed.
        /// </summary>
        public static void PressAnyKey()
        {
            lock (Sync)
            {
                NewPara().Write("Press any key...");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Writes "Press any key..." on a new paragraph, then awaits until any key is pressed.
        /// Works only when debugger is attached, otherwise does nothing.
        /// </summary>
        public static void PressAnyKeyWhenDebug()
        {
#if DEBUG
            if (Debugger.IsAttached)
                PressAnyKey();
#endif
        }
    }

    /// <summary>
    /// Colored console writer.
    /// </summary>
    public partial class XConsoleImpl
    {
        private static bool _lastPara;

        private readonly ConsoleColor? _foreground;
        private readonly ConsoleColor? _background;

        /// <summary>
        /// Initializes new instance.
        /// </summary>
        public XConsoleImpl(ConsoleColor? foreground, ConsoleColor? background)
        {
            _foreground = foreground;
            _background = background;
        }

        /// <summary>
        /// Switches to new console writer with specified colors.
        /// </summary>
        public XConsoleImpl To(ConsoleColor? foreground = null, ConsoleColor? background = null) => new XConsoleImpl(foreground, background);

        /// <summary>
        /// Writes specified string message (or nothing) to the colored output.
        /// </summary>
        public XConsoleImpl Write(string message = null) => WriteColored(message);

        /// <summary>
        /// Writes specified character to the colored output.
        /// </summary>
        public XConsoleImpl Write(char c) => WriteColored(ToString(c));

        /// <summary>
        /// Writes specified string message (or nothing), followed by current line terminator, to the colored output.
        /// </summary>
        public XConsoleImpl WriteLine(string message = null) => WriteColored(ToLine(message));

        private static string ToString(char c) => Char.ToString(c);
        private static string ToLine(string message) => message + Environment.NewLine;

        private XConsoleImpl WriteColored(string message)
        {
            // do nothing if null is passed
            if (message == null)
                return this;

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
            lock (XConsole.Sync)
            {
                foreach (var chunk in chunks)
                {
                    if (Char.IsControl(chunk[0]))
                        Console.Write(chunk);
                    else
                        WriteColoredRaw(chunk);
                }

                _lastPara = false;
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

        /// <summary>
        /// Makes sure the current console position is at the line beginning.
        /// If it is already, does nothing. If it is not, writes line terminator and proceeds to the new line.
        /// </summary>
        public XConsoleImpl NewLine()
        {
            lock (XConsole.Sync)
            {
                if (Console.CursorLeft > 0)
                    Console.WriteLine();
            }

            return this;
        }

        /// <summary>
        /// Adds one blank line and starts from new line.
        /// If current line is not terminated, it will be terminated prior to rendering the blank line.
        /// </summary>
        public XConsoleImpl NewPara()
        {
            lock (XConsole.Sync)
            {
                NewLine();

                if (!_lastPara)
                    Console.WriteLine();

                _lastPara = true;
            }

            return this;
        }
    }
}
