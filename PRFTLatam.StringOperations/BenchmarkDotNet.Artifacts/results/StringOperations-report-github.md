```

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 6.0.413
  [Host]     : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2


```
| Method                         | Mean     | Error    | StdDev   | Rank | Gen0   | Allocated |
|------------------------------- |---------:|---------:|---------:|-----:|-------:|----------:|
| ConcatenatePlusOperator        | 47.58 ns | 0.504 ns | 0.447 ns |    4 | 0.0140 |     176 B |
| ConcatenatePlusEqualOperator   | 37.23 ns | 0.351 ns | 0.293 ns |    2 | 0.0216 |     272 B |
| ConcatenateStringInterpolation | 39.83 ns | 0.237 ns | 0.210 ns |    3 | 0.0070 |      88 B |
| ConcatenateStringFormat        | 89.74 ns | 0.347 ns | 0.307 ns |    6 | 0.0114 |     144 B |
| ConcatenateStringBuilder       | 79.56 ns | 0.378 ns | 0.354 ns |    5 | 0.0331 |     416 B |
| ConcatenateStringConcat        | 31.51 ns | 0.068 ns | 0.061 ns |    1 | 0.0114 |     144 B |
| ConcatenateStringJoin          | 31.81 ns | 0.348 ns | 0.326 ns |    1 | 0.0121 |     152 B |
| ConcatenateLINQ                | 90.52 ns | 0.526 ns | 0.492 ns |    6 | 0.0299 |     376 B |
