# performance
C# Performance Best Practices

# BenchmarkDotNet Results
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.203
  [Host]     : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT
  DefaultJob : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT


```
|                   Method |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |----------:|----------:|----------:|-------:|------:|------:|----------:|
|         Where1LinqQueryX | 651.82 ns |  3.236 ns |  2.868 ns | 0.4320 |     - |     - |     680 B |
|        Where1LinqLambdaX | 652.07 ns | 10.188 ns | 13.601 ns | 0.4330 |     - |     - |     680 B |
|       Where1LinqForeachX | 819.28 ns |  7.691 ns |  6.004 ns | 0.3872 |     - |     - |     608 B |
|           Where1LinqForX | 364.27 ns |  3.793 ns |  5.440 ns | 0.3872 |     - |     - |     608 B |
|   Where1LinqUnrolledForX | 309.84 ns |  2.309 ns |  2.047 ns | 0.3867 |     - |     - |     608 B |
| Where1LinqForVectorizedX |  47.29 ns |  0.496 ns |  0.464 ns | 0.1173 |     - |     - |     184 B |
| Where1LinqForeachSortedX | 336.57 ns |  2.064 ns |  2.294 ns | 0.3867 |     - |     - |     608 B |
|         Where2LinqQueryX | 873.68 ns |  2.495 ns |  2.334 ns | 0.4330 |     - |     - |     680 B |
|        Where2LinqLambdaX | 880.92 ns | 14.421 ns | 14.164 ns | 0.4330 |     - |     - |     680 B |
|       Where2LinqForeachX | 898.01 ns |  4.510 ns |  3.998 ns | 0.3853 |     - |     - |     608 B |
|           Where2LinqForX | 544.19 ns |  5.965 ns |  4.981 ns | 0.3872 |     - |     - |     608 B |
|     Where2LinqForSortedX | 429.06 ns |  8.352 ns |  9.942 ns | 0.3872 |     - |     - |     608 B |
| Where2LinqForVectorizedX | 104.06 ns |  1.169 ns |  1.037 ns | 0.1070 |     - |     - |     168 B |
