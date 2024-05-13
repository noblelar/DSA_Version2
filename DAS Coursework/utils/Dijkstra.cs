using System;
using System.Collections.Generic;
using System.Diagnostics;
using DAS_Coursework.models;

namespace DAS_Coursework.utils
{
    public class Dijkstra
    {
        public static void ShortestPath(TrainSystem graph, Verticex source, Verticex destination)
        {
            // Start the stopwatch to measure the execution time
            Stopwatch stopwatch = Stopwatch.StartNew();

            PriorityQueue pq = new PriorityQueue();
            Dictionary<Verticex, double> distances = new Dictionary<Verticex, double>();
            Dictionary<Verticex, Verticex> predecessors = new Dictionary<Verticex, Verticex>();
            Dictionary<Verticex, WeightedEdge<Verticex>> edgePredecessors = new Dictionary<Verticex, WeightedEdge<Verticex>>(); // Store the edge predecessors for line changes

            // Initialize distances
            foreach (Verticex vertex in graph.Graph().Vertices)
            {
                distances[vertex] = (vertex == source) ? 0 : double.PositiveInfinity;
                pq.Enqueue(vertex, distances[vertex]);
            }

            // Dijkstra's algorithm
            while (!pq.IsEmpty())
            {
                Verticex u = pq.Dequeue();
                if (u == destination) break;

                foreach (WeightedEdge<Verticex> edge in graph.GetOpenEdges())
                {
                    if (edge.Source == u)
                    {
                        double alt = distances[u] + edge.weight;
                        // Consider the additional cost for changing lines
                        if (edgePredecessors.ContainsKey(u) && edgePredecessors[u].line != edge.line)
                        {
                            alt += 2; // Additional cost for changing lines
                        }
                        if (alt < distances[edge.Target])
                        {
                            distances[edge.Target] = alt;
                            predecessors[edge.Target] = u;
                            edgePredecessors[edge.Target] = edge; // Store the edge as predecessor
                            pq.Enqueue(edge.Target, alt);
                        }
                    }
                }
            }

            string endLine = "";
            string endDir = "";
            string startLine = "";
            string startDir = "";

            // Reconstruct shortest path
            Console.WriteLine("\nShortest path from " + source.Name + " to " + destination.Name + ": \n");
            Verticex currentVertex = destination;
            List<string> path = new List<string>();
            while (currentVertex != source)
            {
                Verticex prevVertex = predecessors[currentVertex];
                WeightedEdge<Verticex> connectingEdge = edgePredecessors[currentVertex];
                if (path.Count == 0)
                {
                    endLine = connectingEdge.line;
                    endDir = connectingEdge.direction;
                }

                path.Add($"{connectingEdge.line} ({connectingEdge.direction}): {connectingEdge.Source.Name} to {connectingEdge.Target.Name} {connectingEdge.weight}min");

                // Check for line change
                if (edgePredecessors.ContainsKey(currentVertex) && edgePredecessors.ContainsKey(prevVertex))
                {
                    WeightedEdge<Verticex> prevEdge = edgePredecessors[prevVertex];
                    if (prevEdge != null && connectingEdge != null && prevEdge.line != connectingEdge.line)
                    {
                        path.Add($"Change: {prevEdge.line} to {connectingEdge.line} 2.00min");
                    }
                }

                currentVertex = prevVertex;
                if (currentVertex == null)
                {
                    Console.WriteLine($"There is no path from {source.Name} to {destination.Name}");
                    return;
                }

                startDir = connectingEdge.direction;
                startLine = connectingEdge.line;
            }
            path.Add($"Start: {source.Name}, {startLine} ({startDir})");

            // Print the path
            int pathIndex = path.Count;
            for (int i = pathIndex - 1; i >= 0; i--)
            {
                Console.WriteLine($"\n({pathIndex - i}) {path[i]}");
            }
            Console.WriteLine($"\n({path.Count + 1}) End: {destination.Name}, {endLine} ({endDir})");
            Console.WriteLine($"\nTotal Time: {distances[destination]} minutes");

            stopwatch.Stop();
            Console.WriteLine($"\nElapsed Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
