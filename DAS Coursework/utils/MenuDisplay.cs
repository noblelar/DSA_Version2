using System;
using Spectre.Console;

namespace DAS_Coursework.utils
{
    public static class MenuDisplay
    {
        public static int GetMenu(string[] options, string[] message)
        {
            int selectedLineIndex = 0;
            ConsoleKey pressedKey;
            do
            {
                UpdateMenu(selectedLineIndex, options, message);
                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex + 1 < options.Length)
                    selectedLineIndex++;

                else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex - 1 >= 0)
                    selectedLineIndex--;



            } while (pressedKey != ConsoleKey.Enter);

            Console.ResetColor();

            return selectedLineIndex;
        }

        static void UpdateMenu(int index, string[] options, string[] messages)
        {
            AnsiConsole.Clear();
            TitleCreator.GetTitle();

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            Console.WriteLine();

            foreach (var option in options)
            {
                bool isSelected = option == options[index];
                ChangeLineColor(isSelected);
                Console.WriteLine($"{(isSelected ? "> " : "  ")}{option}");
                Console.ResetColor();
            }
        }

        static void ChangeLineColor(bool shouldHighlight)
        {
            if (shouldHighlight)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ResetColor();
            }
        }
    }
}

