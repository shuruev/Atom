using System;

namespace Atom.Util
{
    partial class XConsole
    {
        /// <summary>
        /// Default color style: no specifying foreground or background colors.
        /// </summary>
        public static XConsoleImpl Default { get; } = To();

        /// <summary>
        /// Resets console colors to the default ones.
        /// </summary>
        public static XConsoleImpl Reset()
        {
            Console.ResetColor();
            return Default;
        }

        //
        // Standard:
        //

        public static XConsoleImpl Red { get; } = To(ConsoleColor.Red);
        public static XConsoleImpl Green { get; } = To(ConsoleColor.Green);
        public static XConsoleImpl Blue { get; } = To(ConsoleColor.Blue);
        public static XConsoleImpl Cyan { get; } = To(ConsoleColor.Cyan);
        public static XConsoleImpl Yellow { get; } = To(ConsoleColor.Yellow);
        public static XConsoleImpl Gold { get; } = To(ConsoleColor.DarkYellow);
        public static XConsoleImpl Muted { get; } = To(ConsoleColor.DarkGray);
        public static XConsoleImpl Bright { get; } = To(ConsoleColor.White);

        //
        // Messaging:
        //

        public static XConsoleImpl Error { get; } = To(ConsoleColor.White, ConsoleColor.DarkRed);
        public static XConsoleImpl OK { get; } = To(ConsoleColor.White, ConsoleColor.DarkGreen);
        public static XConsoleImpl Warning { get; } = To(ConsoleColor.Black, ConsoleColor.DarkYellow);
        public static XConsoleImpl Tag { get; } = To(ConsoleColor.Black, ConsoleColor.Gray);
        public static XConsoleImpl Note { get; } = To(ConsoleColor.Black, ConsoleColor.White);
        public static XConsoleImpl Tip { get; } = To(ConsoleColor.Black, ConsoleColor.Yellow);
        public static XConsoleImpl Label { get; } = To(ConsoleColor.Yellow, ConsoleColor.DarkCyan);
        public static XConsoleImpl Terminal { get; } = To(ConsoleColor.White, ConsoleColor.DarkBlue);

        //
        // Extended:
        //

        public static XConsoleImpl Graphite { get; } = To(ConsoleColor.Gray, ConsoleColor.DarkGray);
        public static XConsoleImpl Azure { get; } = To(ConsoleColor.DarkBlue, ConsoleColor.Cyan);
        public static XConsoleImpl Eggplant { get; } = To(ConsoleColor.Yellow, ConsoleColor.Magenta);
        public static XConsoleImpl Strawberry { get; } = To(ConsoleColor.Green, ConsoleColor.Red);
        public static XConsoleImpl Watermelon { get; } = To(ConsoleColor.DarkRed, ConsoleColor.Green);
        public static XConsoleImpl Squash { get; } = To(ConsoleColor.Yellow, ConsoleColor.DarkYellow);
        public static XConsoleImpl Lime { get; } = To(ConsoleColor.Yellow, ConsoleColor.Green);
        public static XConsoleImpl Tomato { get; } = To(ConsoleColor.Cyan, ConsoleColor.DarkRed);

        /// <summary>
        /// Prints all built-in styles for quick browsing.
        /// </summary>
        public static void PrintDemo()
        {
            NewPara();

            Write("Standard:   ")
                .Red.Write("Red").Default.Write("         ")
                .Green.Write("Green").Default.Write("    ")
                .Blue.Write("Blue").Default.Write("        ")
                .Cyan.Write("Cyan").Default.Write("          ")
                .Yellow.Write("Yellow").Default.Write("        ")
                .Gold.Write("Gold").Default.Write("      ")
                .Muted.Write("Muted").Default.Write("    ")
                .Bright.WriteLine("Bright");

            Write("Messaging:  ")
                .Error.Write("Error").Default.Write("       ")
                .OK.Write("OK").Default.Write("       ")
                .Warning.Write("Warning").Default.Write("     ")
                .Tag.Write("Tag").Default.Write("           ")
                .Note.Write("Note").Default.Write("          ")
                .Tip.Write("Tip").Default.Write("       ")
                .Label.Write("Label").Default.Write("    ")
                .Terminal.WriteLine("Terminal");

            Write("Extended:   ")
                .Graphite.Write("Graphite").Default.Write("    ")
                .Azure.Write("Azure").Default.Write("    ")
                .Eggplant.Write("Eggplant").Default.Write("    ")
                .Strawberry.Write("Strawberry").Default.Write("    ")
                .Watermelon.Write("Watermelon").Default.Write("    ")
                .Squash.Write("Squash").Default.Write("    ")
                .Lime.Write("Lime").Default.Write("     ")
                .Tomato.WriteLine("Tomato");

            NewPara();

            Console.WriteLine("Standard:\tMessaging:\tExtended:");
            Red.Write("Red\t\t").Error.Write("Error\t\t").Graphite.WriteLine("Graphite");
            Green.Write("Green\t\t").OK.Write("OK\t\t").Azure.WriteLine("Azure");
            Blue.Write("Blue\t\t").Warning.Write("Warning\t\t").Eggplant.WriteLine("Eggplant");
            Cyan.Write("Cyan\t\t").Tag.Write("Tag\t\t").Strawberry.WriteLine("Strawberry");
            Yellow.Write("Yellow\t\t").Tip.Write("Tip\t\t").Squash.WriteLine("Squash");
            Gold.Write("Gold\t\t").Note.Write("Note\t\t").Watermelon.WriteLine("Watermelon");
            Muted.Write("Muted\t\t").Label.Write("Label\t\t").Lime.WriteLine("Lime");
            Bright.Write("Bright\t\t").Terminal.Write("Terminal\t").Tomato.WriteLine("Tomato");

            NewPara();
        }
    }

    partial class XConsoleImpl
    {
        /// <summary>
        /// Switches to default color style: no specifying foreground or background colors.
        /// </summary>
        public XConsoleImpl Default => XConsole.Default;

        //
        // Standard:
        //

        public XConsoleImpl Red => XConsole.Red;
        public XConsoleImpl Green => XConsole.Green;
        public XConsoleImpl Blue => XConsole.Blue;
        public XConsoleImpl Cyan => XConsole.Cyan;
        public XConsoleImpl Yellow => XConsole.Yellow;
        public XConsoleImpl Gold => XConsole.Gold;
        public XConsoleImpl Muted => XConsole.Muted;
        public XConsoleImpl Bright => XConsole.Bright;

        //
        // Messaging:
        //

        public XConsoleImpl Error => XConsole.Error;
        public XConsoleImpl OK => XConsole.OK;
        public XConsoleImpl Warning => XConsole.Warning;
        public XConsoleImpl Tag => XConsole.Tag;
        public XConsoleImpl Note => XConsole.Note;
        public XConsoleImpl Tip => XConsole.Tip;
        public XConsoleImpl Label => XConsole.Label;
        public XConsoleImpl Terminal => XConsole.Terminal;

        //
        // Extended:
        //

        public XConsoleImpl Graphite => XConsole.Graphite;
        public XConsoleImpl Azure => XConsole.Azure;
        public XConsoleImpl Eggplant => XConsole.Eggplant;
        public XConsoleImpl Strawberry => XConsole.Strawberry;
        public XConsoleImpl Watermelon => XConsole.Watermelon;
        public XConsoleImpl Squash => XConsole.Squash;
        public XConsoleImpl Lime => XConsole.Lime;
        public XConsoleImpl Tomato => XConsole.Tomato;
    }
}
