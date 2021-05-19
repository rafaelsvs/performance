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
|                   Method |        Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |------------:|---------:|---------:|-------:|------:|------:|----------:|
|         Where1LinqQueryX |   716.00 ns | 3.010 ns | 2.668 ns | 0.4320 |     - |     - |     680 B |
|        Where1LinqLambdaX |   641.37 ns | 2.511 ns | 2.226 ns | 0.4320 |     - |     - |     680 B |
|       Where1LinqForeachX |   757.85 ns | 4.653 ns | 4.124 ns | 0.3853 |     - |     - |     608 B |
|           Where1LinqForX |   250.21 ns | 2.240 ns | 1.986 ns | 0.3619 |     - |     - |     568 B |
|   Where1LinqUnrolledForX |   223.04 ns | 1.537 ns | 1.284 ns | 0.3619 |     - |     - |     568 B |
| Where1LinqForVectorizedX |    47.28 ns | 0.458 ns | 0.406 ns | 0.1173 |     - |     - |     184 B |
| Where1LinqForeachSortedX |   227.44 ns | 2.477 ns | 2.068 ns | 0.3619 |     - |     - |     568 B |
|         Where2LinqQueryX |   902.05 ns | 5.163 ns | 4.576 ns | 0.4330 |     - |     - |     680 B |
|        Where2LinqLambdaX |   865.95 ns | 5.297 ns | 4.135 ns | 0.4330 |     - |     - |     680 B |
|       Where2LinqForeachX | 1,027.95 ns | 6.603 ns | 5.853 ns | 0.3872 |     - |     - |     608 B |
|           Where2LinqForX |   393.32 ns | 7.540 ns | 7.405 ns | 0.3619 |     - |     - |     568 B |
|     Where2LinqForSortedX |   313.08 ns | 1.106 ns | 0.924 ns | 0.3619 |     - |     - |     568 B |
| Where2LinqForVectorizedX |   101.43 ns | 0.392 ns | 0.347 ns | 0.1070 |     - |     - |     168 B |
