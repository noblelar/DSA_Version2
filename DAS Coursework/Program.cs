namespace DAS_Coursework;
using controller;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<DijkstraBenchmark>(new CustomConfig());
    }

}

