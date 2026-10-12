using NUnit.Framework;

// Each feature file runs as its own fixture, so features run in parallel.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]
