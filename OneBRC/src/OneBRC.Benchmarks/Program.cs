using BenchmarkDotNet.Running;

using OneBRC.Benchmarks;

using static OneBRC.Benchmarks.CityBenchmark;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
