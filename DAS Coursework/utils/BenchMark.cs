using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using DAS_Coursework.models;

namespace DAS_Coursework.utils
{
    public class DijkstraBenchmark
    {
        private TrainSystem graph;
        private Verticex source;
        private Verticex destination;

        [GlobalSetup]
        public void Setup()
        {
            // Initialize your train system, source, and destination vertices
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

            source = graph.FindVertexByName("WEMBLEY CENTRAL");
            destination = graph.FindVertexByName("VICTORIA");
        }

        [Benchmark]
        public void BenchmarkShortestPath()
        {
            Dijkstra.ShortestPath(graph, source, destination);
        }
    }

    public class CustomConfig : ManualConfig
    {
        public CustomConfig()
        {
            AddValidator(JitOptimizationsValidator.DontFailOnError); // Disables the optimizations validator warning
            AddLogger(ConsoleLogger.Default); // Adds console logger to see progress
            AddColumnProvider(DefaultColumnProviders.Instance); // Adds default columns
        }
    }

}