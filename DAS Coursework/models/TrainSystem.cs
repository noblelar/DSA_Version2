using System;
using System.Collections.Generic;
using System.Linq;
using DAS_Coursework.models;
using QuikGraph;


namespace DAS_Coursework.models
{
    public class TrainSystem
    {
        public List<Delay> delays;
        AdjacencyGraph<Verticex, WeightedEdge<Verticex>> graph;

        public TrainSystem()
        {
            delays = new List<Delay>();
            graph = new AdjacencyGraph<Verticex, WeightedEdge<Verticex>>(); ;
        }

        public void AddVertex(string vertexName)
        {
            if (string.IsNullOrWhiteSpace(vertexName) || graph.Vertices.Any(v => v.Name == vertexName.Trim()))
                return;

            graph.AddVertex(new Verticex(vertexName.Trim()));
        }

        public string[] GetAllVertexNames()
        {
            return graph.Vertices.Select(v => v.Name).ToArray();
        }

        public List<WeightedEdge<Verticex>> GetOpenEdges()
        {
            return graph.Edges.Where(e => !e.isClosed).ToList();
        }

        public IEnumerable<Verticex> Vertices()
        {
            return graph.Vertices;
        }

        public AdjacencyGraph<Verticex, WeightedEdge<Verticex>> Graph()
        {
            return graph;
        }

        public void AddEdge(string line, string fromVertexName, string toVertexName, double unimpeded, string direction, double amPeak)
        {
            Verticex fromVertex = FindVertexByName(fromVertexName.Trim());
            Verticex toVertex = FindVertexByName(toVertexName.Trim());

            if (fromVertex == null || toVertex == null)
                return;

            graph.AddEdge(new WeightedEdge<Verticex>(line.Trim(), fromVertex, toVertex, unimpeded == 0 ? amPeak : unimpeded, direction));
        }

        public void GetGraph()
        {
            Console.WriteLine("Vertices:");

            foreach (var node in graph.Vertices)
            {
                Console.WriteLine(node.Name);
            }

        }

        public void GetEdges()
        {
            Console.WriteLine("Edges:");
            foreach (var edge in graph.Edges)
            {
                Console.WriteLine($"{edge.Source.Name} -> {edge.Target.Name}, Weight: {edge.weight}");
            }
        }

        public Verticex FindVertexByName(string vertexName)
        {
            return graph.Vertices.FirstOrDefault(v => v.Name == vertexName.Trim());
        }

        public string[] VerticesConnectedToLine(string lineName)
        {
            var connectedVertices = new HashSet<string>();

            foreach (var edge in graph.Edges.Where(e => e.line == lineName))
            {
                connectedVertices.Add(edge.Source.Name);
                connectedVertices.Add(edge.Target.Name);
            }

            return connectedVertices.ToArray();
        }

        public string[] FindLineDirections(string lineName)
        {
            return graph.Edges.Where(e => e.line == lineName).Select(e => e.direction).Distinct().ToArray();
        }

        public WeightedEdge<Verticex> GetEdgeWithStartVertexAndLine(string startVertexName, string lineName)
        {
            return graph.Edges.FirstOrDefault(e => e.line == lineName && e.Target.Name == startVertexName);
        }

        public WeightedEdge<Verticex> GetEdge(string startVertexName, string lineName)
        {
            return graph.Edges.FirstOrDefault(e => e.line == lineName && e.Source.Name == startVertexName);
        }

        public bool AddDelayToEdge(string startVertexName, string lineName, double delay, string direction, string endVertexName)
        {
            var delayingEdge = FindVertexByName(startVertexName);
            var ending = FindVertexByName(endVertexName);

            if (delayingEdge == null || ending == null)
                return false;

            if (direction != "BOTH")
            {
                var path = FindPathBetweenStations(startVertexName, endVertexName, lineName, direction, false);
                if (path.Length == 0)
                {
                    Console.WriteLine($"There is no path from {startVertexName} to {endVertexName} on the {lineName} line");
                    return false;
                }

                var newDelay = new Delay(delay, FindVertexByName(startVertexName), FindVertexByName(endVertexName), direction, lineName);
                path[0].AddDelay(delay);
                delays.Add(newDelay);

            }
            else
            {
                string[] directionOption = FindLineDirections(lineName);

                WeightedEdge<Verticex>[] start;
                WeightedEdge<Verticex>[] end;

                start = FindPathBetweenStations(startVertexName, endVertexName, lineName, directionOption[0], false);

                if (start.Length == 0)
                {
                    start = FindPathBetweenStations(endVertexName, startVertexName, lineName, directionOption[0], false);
                }

                end = FindPathBetweenStations(startVertexName, endVertexName, lineName, directionOption[1], false);

                if (end.Length == 0)
                {
                    end = FindPathBetweenStations(endVertexName, startVertexName, lineName, directionOption[1], false);
                }

                if (end.Length != 0 && start.Length != 0)
                {
                    start[0].AddDelay(delay);
                    end[0].AddDelay(delay);

                    var newDelay = new Delay(delay, FindVertexByName(startVertexName), FindVertexByName(endVertexName), start[0].direction, lineName);
                    delays.Add(newDelay);

                    newDelay = new Delay(delay, FindVertexByName(endVertexName), FindVertexByName(startVertexName), end[0].direction, lineName);
                    delays.Add(newDelay);
                }
                else
                {
                    Console.WriteLine($"There is no path from {startVertexName} to {endVertexName} on the {lineName} line");
                    return false;
                }
            }

            return true;
        }

        public Delay? FindDelay(string startStationName, string endStationName, string lineName, string direction)
        {
            return delays.FirstOrDefault(delay =>
                delay.StartStation.Name == startStationName &&
                delay.EndStation.Name == endStationName &&
                delay.Line == lineName && delay.Direction == direction);
        }

        public bool RemoveDelay(Delay delayToRemove)
        {
            delays.Remove(delayToRemove);
            return true;
        }

        public WeightedEdge<Verticex>[] FindPathBetweenStations(string startVertexName, string endVertexName, string lineName, string direction, bool status)
        {
            var startVertex = FindVertexByName(startVertexName);
            var endVertex = FindVertexByName(endVertexName);
            var path = new List<WeightedEdge<Verticex>>();
            var visited = new bool[graph.Vertices.Count()];

            if (startVertex == null || endVertex == null)
                return new WeightedEdge<Verticex>[0];

            DFS(startVertex, endVertex, lineName, visited, path, direction, status);

            return path.ToArray();
        }
        private bool DFS(Verticex currentVertex, Verticex endVertex, string lineName, bool[] visited, List<WeightedEdge<Verticex>> path, string direction, bool status)
        {
            visited[Array.IndexOf(graph.Vertices.ToArray(), currentVertex)] = true;

            if (currentVertex == endVertex)
                return true;

            foreach (var edge in graph.Edges)
            {
                if (edge.line == lineName && edge.isClosed == status && edge.direction == direction && edge.Source == currentVertex && !visited[Array.IndexOf(graph.Vertices.ToArray(), edge.Target)])
                {
                    path.Add(edge);

                    if (DFS(edge.Target, endVertex, lineName, visited, path, direction, status))
                        return true;

                    path.Remove(edge);
                }
            }

            return false;
        }

        public bool CloseEdges(WeightedEdge<Verticex>[] path)
        {
            foreach (var edge in path)
            {
                edge.Close();
            }
            return true;
        }

        public bool OpenEdges(WeightedEdge<Verticex>[] path)
        {
            foreach (var edge in path)
            {
                edge.Open();
            }
            return true;
        }

        public WeightedEdge<Verticex>[] FindTracks(string lineName, string direction, bool isClose)
        {
            return graph.Edges.Where(e => e.line == lineName && e.direction == direction && e.isClosed == isClose).ToArray();
        }

        public (Verticex, WeightedEdge<Verticex>[], string[]) GetVertexAndEdges(string vertexName)
        {
            var vertex = FindVertexByName(vertexName);
            var connectingEdges = graph.Edges.Where(e => e.Source == vertex || e.Target == vertex).ToArray();
            var uniqueLines = connectingEdges.Select(e => e.line).Distinct().ToArray();

            return (vertex, connectingEdges, uniqueLines);
        }

    }
}
