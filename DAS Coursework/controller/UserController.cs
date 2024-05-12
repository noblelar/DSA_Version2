using System;
using DAS_Coursework.utils;
using Spectre.Console;

namespace DAS_Coursework.controller
{
    public static class UserController
    {
        public static void GetUserMenu()
        {
            string[] UserOptions = new[]{
                    "Find A Route",
                    "Display information about a station",
                    "Go Back"
            };

            int response = MenuDisplay.GetMenu(UserOptions, new[] { "This is the customer menu", "What action do you want to perform:" });

            if (response == 2)
            {
                MainController.GetMainMain();
            }
            else if (response == 1)
            {
                GetDisplayInformationMenu();
            }
            else
            {
                GetDisplayRouteMenu();
            }
        }


        public static void GetDisplayInformationMenu()
        {
         

            AnsiConsole.Clear();
            TitleCreator.GetTitle();
            string[] StationOptions = MainController.graph.GetAllVertexNames().Append("Cancel").ToArray();
            var station = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Find A Route \nPlease select your station?")
                        .PageSize(20)
                        .MoreChoicesText("[grey](Move up and down to reveal more stations)[/]")
                        .AddChoices(StationOptions));

            if (station == "Cancel")
            {
                GetUserMenu();
                return;
            }


            AnsiConsole.Clear();
            TitleCreator.GetTitle();
            Console.WriteLine($"\nBelow is the information about {station} station\n");
            var response = MainController.graph.GetVertexAndEdges(station);

            Console.WriteLine($"Station Name: {response.Item1.Name}\n");
            Console.WriteLine("Train Lines available:");
            for(var i = 0; i < response.Item3.Length; i++)
            {
                Console.WriteLine($"{i+1}. {response.Item3[i]}");
            }
            Console.WriteLine();
            Console.WriteLine("This station connects to or is connected to:");
            for (var i = 0; i < response.Item2.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {response.Item2[i].Source.Name} -> {response.Item2[i].Target.Name} {response.Item2[i].line}({response.Item2[i].direction}) (Journey time: {response.Item2[i].weight} mins{(response.Item2[i].delay>0 ?$" + Delay: {response.Item2[i].delay} min":"")})");
            }

            Console.WriteLine("\nPress enter to go back");
            ConsoleKey pressedKey = Console.ReadKey().Key;

            if (pressedKey == ConsoleKey.Enter)
            {
                GetUserMenu();
                return;
            }
        }


        public static void DisplayStation(string station)
        {
            AnsiConsole.Clear();
            TitleCreator.GetTitle();

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(station);
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine("Lines available are:");

            Console.WriteLine("\nPress enter to go back");
            ConsoleKey pressedKey = Console.ReadKey().Key;

            if (pressedKey == ConsoleKey.Enter)
            {
                GetUserMenu();
                return;
            }

        }

        public static void GetDisplayRouteMenu()
        {
            string[] StationOptions = MainController.graph.GetAllVertexNames().Append("Cancel").ToArray();

            var startStation = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Find A Route \nPlease select your start station:?")
                        .PageSize(20)
                        .MoreChoicesText("[grey](Move up and down to reveal more stations)[/]")
                        .AddChoices(StationOptions));

            if (startStation == "Cancel")
            {
                GetUserMenu();
                return;
            }

            string[] EndOptions = StationOptions.Where(s => !s.Contains(startStation)).ToArray();
            var endStation = AnsiConsole.Prompt(
                   new SelectionPrompt<string>()
                       .Title($"Find A Route \nPlease select your ending station: \n\nStart destination: {startStation}")
                       .PageSize(20)
                       .MoreChoicesText("[grey](Move up and down to reveal more stations)[/]")
                       .AddChoices(StationOptions));

            if (endStation == "Cancel")
            {
                GetUserMenu();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n\nThe fastest route from {startStation} to {endStation}");
            Console.ResetColor();
            Dijkstra.ShortestPath(MainController.graph, MainController.graph.FindVertexByName(startStation), MainController.graph.FindVertexByName(endStation));

            Console.WriteLine("\nPress enter to go back");
            ConsoleKey pressedKey = Console.ReadKey().Key;

            if (pressedKey == ConsoleKey.Enter)
            {
                GetUserMenu();
            }
        }

    }
}

