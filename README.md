# performance
C# Performance Best Practices

# Where1

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.203
  [Host]     : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT
  DefaultJob : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT


```
|        Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
|-------------- |----------:|----------:|----------:|------:|-------:|------:|------:|----------:|----------:|
|     LinqQuery | 717.29 ns |  3.509 ns |  3.111 ns |  1.00 | 0.4330 |     - |     - |     680 B |    1221 B |
|        Lambda | 670.56 ns | 12.151 ns | 10.146 ns |  0.93 | 0.4320 |     - |     - |     680 B |    1221 B |
|       Foreach | 777.00 ns |  6.240 ns |  6.129 ns |  1.08 | 0.3853 |     - |     - |     608 B |     622 B |
|           For | 226.71 ns |  2.256 ns |  2.111 ns |  0.32 | 0.3619 |     - |     - |     568 B |     611 B |
|   UnrolledFor | 218.02 ns |  3.269 ns |  3.058 ns |  0.30 | 0.3617 |     - |     - |     568 B |    1007 B |
| ForVectorized |  50.91 ns |  0.692 ns |  0.614 ns |  0.07 | 0.1172 |     - |     - |     184 B |     838 B |
| ForeachSorted | 224.16 ns |  3.653 ns |  3.417 ns |  0.31 | 0.3619 |     - |     - |     568 B |     775 B |

# Where2
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.203
  [Host]     : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT
  DefaultJob : .NET Core 3.1.15 (CoreCLR 4.700.21.21202, CoreFX 4.700.21.21402), X64 RyuJIT


```
|        Method |     Mean |   Error |  StdDev | Ratio | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------:|--------:|--------:|------:|----------:|-------:|------:|------:|----------:|
|     LinqQuery | 854.0 ns | 5.63 ns | 4.70 ns |  1.00 |    1313 B | 0.4330 |     - |     - |     680 B |
|        Lambda | 899.5 ns | 3.79 ns | 3.55 ns |  1.05 |    1313 B | 0.4330 |     - |     - |     680 B |
|       Foreach | 961.3 ns | 5.72 ns | 5.35 ns |  1.12 |     801 B | 0.3872 |     - |     - |     608 B |
|           For | 391.8 ns | 3.28 ns | 2.74 ns |  0.46 |     803 B | 0.3619 |     - |     - |     568 B |
|     ForSorted | 310.7 ns | 2.62 ns | 2.19 ns |  0.36 |     823 B | 0.3619 |     - |     - |     568 B |
| ForVectorized | 108.3 ns | 1.89 ns | 2.02 ns |  0.13 |    1437 B | 0.1070 |     - |     - |     168 B |
