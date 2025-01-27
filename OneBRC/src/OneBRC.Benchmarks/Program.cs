using BenchmarkDotNet.Running;

using static OneBRC.Benchmarks.CityBenchmark;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

// var x = CityKey.Create("1234567890abcde"u8);