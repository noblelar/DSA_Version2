namespace DAS_Coursework;
using controller;
using BenchmarkDotNet.Running;

using utils;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<DijkstraBenchmark>(new CustomConfig());
    }

}

