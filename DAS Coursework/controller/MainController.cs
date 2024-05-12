using System;
using DAS_Coursework.models;
using DAS_Coursework.utils;

namespace DAS_Coursework.controller
{
    public static class MainController
    {

        public static TrainSystem graph;

        public static void GetMainMain()
        {
            string[] UserTypes = new[]{
                                    "Engineer",
                                    "Customer",
                                    "Exit"
                                };

            int response = MenuDisplay.GetMenu(UserTypes, new[] { "What Kind of User Are you" });

            Console.WriteLine();

            if (response == 0)
            {
                EngineerController.GetEngineerMenu();
            }
            else if (response == 1)
            {
                UserController.GetUserMenu();
            }
            else
            {
                Environment.Exit(0);
            }

            Console.ReadKey();

        }

        public static void Init()
        {
            graph = new TrainSystem();

            var travels = data.GetData.GetAllTrainData();

            foreach (var travel in travels)
            {
                graph.AddVertex(travel.StationA);
            }

            foreach (var travel in travels)
            {
                graph.AddEdge(travel.Line, travel.StationA, travel.StationB, travel.UmimpededTime, travel.Direction, travel.AmPeakTime);
            }

        }
    }
}

