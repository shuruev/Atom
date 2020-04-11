﻿using System;

namespace Atom.Util
{
    partial class XConsole
    {
        /// <summary>
        /// Default placeholder for extending with custom styles (see examples on GitHub).
        /// </summary>
        public static readonly XConsole Style = With();

        /// <summary>
        /// Switches to current color style: no specifying foreground or background colors.
        /// </summary>
        public XConsole ToCurrent => Style;

        /// <summary>
        /// Default color style: Gray on Black
        /// </summary>
        public static readonly XConsole Default = With(ConsoleColor.Gray, ConsoleColor.Black);

        /// <summary>
        /// Switches to default color style: Gray on Black
        /// </summary>
        public XConsole ToDefault => Default;

        //
        // Standard:
        //

        public static readonly XConsole Red = With(ConsoleColor.DarkRed);
        public static readonly XConsole Green = With(ConsoleColor.DarkGreen);
        public static readonly XConsole Blue = With(ConsoleColor.DarkBlue);
        public static readonly XConsole Cyan = With(ConsoleColor.Cyan);
        public static readonly XConsole Gold = With(ConsoleColor.DarkYellow);
        public static readonly XConsole Purple = With(ConsoleColor.Magenta);
        public static readonly XConsole Muted = With(ConsoleColor.DarkGray);
        public static readonly XConsole Bright = With(ConsoleColor.White);

        public XConsole ToRed => Red;
        public XConsole ToGreen => Green;
        public XConsole ToBlue => Blue;
        public XConsole ToCyan => Cyan;
        public XConsole ToGold => Gold;
        public XConsole ToPurple => Purple;
        public XConsole ToMuted => Muted;
        public XConsole ToBright => Bright;

        //
        // Messaging:
        //

        public static readonly XConsole Error = new XConsole(ConsoleColor.White, ConsoleColor.DarkRed);
        public static readonly XConsole OK = new XConsole(ConsoleColor.White, ConsoleColor.DarkGreen);
        public static readonly XConsole Warning = With(ConsoleColor.Black, ConsoleColor.DarkYellow);
        public static readonly XConsole Tag = new XConsole(ConsoleColor.Black, ConsoleColor.Gray);
        public static readonly XConsole Note = new XConsole(ConsoleColor.Black, ConsoleColor.White);
        public static readonly XConsole Tip = new XConsole(ConsoleColor.Black, ConsoleColor.Yellow);
        public static readonly XConsole Label = new XConsole(ConsoleColor.Yellow, ConsoleColor.DarkCyan);
        public static readonly XConsole Terminal = With(ConsoleColor.White, ConsoleColor.DarkBlue);

        public XConsole ToError => Error;
        public XConsole ToOK => OK;
        public XConsole ToWarning => Warning;
        public XConsole ToTag => Tag;
        public XConsole ToNote => Note;
        public XConsole ToTip => Tip;
        public XConsole ToLabel => Label;
        public XConsole ToTerminal => Terminal;

        //
        // Extended:
        //

        public static readonly XConsole Graphite = With(ConsoleColor.Gray, ConsoleColor.DarkGray);
        public static readonly XConsole Azure = new XConsole(ConsoleColor.DarkBlue, ConsoleColor.Cyan);
        public static readonly XConsole Eggplant = new XConsole(ConsoleColor.Yellow, ConsoleColor.Magenta);
        public static readonly XConsole Strawberry = new XConsole(ConsoleColor.Green, ConsoleColor.Red);
        public static readonly XConsole Watermelon = new XConsole(ConsoleColor.DarkRed, ConsoleColor.Green);
        public static readonly XConsole Squash = new XConsole(ConsoleColor.Yellow, ConsoleColor.DarkYellow);
        public static readonly XConsole Lime = new XConsole(ConsoleColor.Yellow, ConsoleColor.Green);
        public static readonly XConsole Tomato = new XConsole(ConsoleColor.Cyan, ConsoleColor.DarkRed);

        public XConsole ToGraphite => Graphite;
        public XConsole ToAzure => Azure;
        public XConsole ToEggplant => Eggplant;
        public XConsole ToStrawberry => Strawberry;
        public XConsole ToWatermelon => Watermelon;
        public XConsole ToSquash => Squash;
        public XConsole ToLime => Lime;
        public XConsole ToTomato => Tomato;

        /// <summary>
        /// Prints all built-in styles for quick browsing.
        /// </summary>
        public static void PrintDemo()
        {
            Console.WriteLine();
            Console.WriteLine();

            Write("Standard:   ")
                .ToRed.Write("Red").ToCurrent.Write("         ")
                .ToGreen.Write("Green").ToCurrent.Write("    ")
                .ToBlue.Write("Blue").ToCurrent.Write("        ")
                .ToCyan.Write("Cyan").ToCurrent.Write("          ")
                .ToGold.Write("Gold").ToCurrent.Write("          ")
                .ToPurple.Write("Purple").ToCurrent.Write("    ")
                .ToMuted.Write("Muted").ToCurrent.Write("    ")
                .ToBright.WriteLine("Bright");

            Write("Messaging:  ")
                .ToError.Write("Error").ToCurrent.Write("       ")
                .ToOK.Write("OK").ToCurrent.Write("       ")
                .ToWarning.Write("Warning").ToCurrent.Write("     ")
                .ToTag.Write("Tag").ToCurrent.Write("           ")
                .ToNote.Write("Note").ToCurrent.Write("          ")
                .ToTip.Write("Tip").ToCurrent.Write("       ")
                .ToLabel.Write("Label").ToCurrent.Write("    ")
                .ToTerminal.WriteLine("Terminal");

            Write("Extended:   ")
                .ToGraphite.Write("Graphite").ToCurrent.Write("    ")
                .ToAzure.Write("Azure").ToCurrent.Write("    ")
                .ToEggplant.Write("Eggplant").ToCurrent.Write("    ")
                .ToStrawberry.Write("Strawberry").ToCurrent.Write("    ")
                .ToWatermelon.Write("Watermelon").ToCurrent.Write("    ")
                .ToSquash.Write("Squash").ToCurrent.Write("    ")
                .ToLime.Write("Lime").ToCurrent.Write("     ")
                .ToTomato.WriteLine("Tomato");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Standard:\tMessaging:\tExtended:");
            Red.Write("Red\t\t").ToError.Write("Error\t\t").ToGraphite.WriteLine("Graphite");
            Green.Write("Green\t\t").ToOK.Write("OK\t\t").ToAzure.WriteLine("Azure");
            Blue.Write("Blue\t\t").ToWarning.Write("Warning\t\t").ToEggplant.WriteLine("Eggplant");
            Cyan.Write("Cyan\t\t").ToTag.Write("Tag\t\t").ToStrawberry.WriteLine("Strawberry");
            Gold.Write("Gold\t\t").ToNote.Write("Note\t\t").ToWatermelon.WriteLine("Watermelon");
            Purple.Write("Purple\t\t").ToTip.Write("Tip\t\t").ToSquash.WriteLine("Squash");
            Muted.Write("Muted\t\t").ToLabel.Write("Label\t\t").ToLime.WriteLine("Lime");
            Bright.Write("Bright\t\t").ToTerminal.Write("Terminal\t").ToTomato.WriteLine("Tomato");

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}