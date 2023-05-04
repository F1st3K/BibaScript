using System;

namespace ConsoleColor
{
    public static class View
    {
        public static void ColorWriteLine(string value, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultColor;
        }

        public static void ColorWriteLine(string value, ConsoleColor foreground, ConsoleColor background)
        {
            var defaultForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            var defaultBackground = Console.BackgroundColor;
            Console.BackgroundColor = background;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
        }

        public static void ColorWrite(string value, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultColor;
        }

        public static void ColorWrite(string value, ConsoleColor foreground, ConsoleColor background)
        {
            var defaultForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            var defaultBackground = Console.BackgroundColor;
            Console.BackgroundColor = background;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
        }

        public static string ColorReadLine(ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            var output = Console.ReadLine();
            Console.ForegroundColor = defaultColor;
            return output;
        }

        public static string ColorReadLine(ConsoleColor foreground, ConsoleColor background)
        {
            var defaultForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            var defaultBackground = Console.BackgroundColor;
            Console.BackgroundColor = background;
            var output = Console.ReadLine();
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
            return output;
        }
    }
}
