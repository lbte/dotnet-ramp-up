```

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 6.0.414
  [Host]     : .NET 6.0.22 (6.0.2223.42425), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.22 (6.0.2223.42425), X64 RyuJIT AVX2


```
| Method                         | Mean      | Error    | StdDev   | Median    | Rank | Gen0   | Allocated |
|------------------------------- |----------:|---------:|---------:|----------:|-----:|-------:|----------:|
| ConcatenatePlusOperator        |  61.80 ns | 1.275 ns | 1.518 ns |  62.18 ns |    4 | 0.0139 |     176 B |
| ConcatenatePlusEqualOperator   |  49.83 ns | 1.059 ns | 2.864 ns |  48.91 ns |    3 | 0.0216 |     272 B |
| ConcatenateStringInterpolation |  48.51 ns | 0.742 ns | 0.658 ns |  48.49 ns |    3 | 0.0070 |      88 B |
| ConcatenateStringFormat        | 111.47 ns | 2.170 ns | 2.132 ns | 111.32 ns |    6 | 0.0114 |     144 B |
| ConcatenateStringBuilder       | 102.17 ns | 1.282 ns | 1.200 ns | 102.48 ns |    5 | 0.0331 |     416 B |
| ConcatenateStringConcat        |  39.53 ns | 0.365 ns | 0.324 ns |  39.50 ns |    1 | 0.0114 |     144 B |
| ConcatenateStringJoin          |  41.04 ns | 0.510 ns | 0.477 ns |  40.97 ns |    2 | 0.0121 |     152 B |
| ConcatenateLINQ                | 118.53 ns | 1.803 ns | 1.598 ns | 118.05 ns |    7 | 0.0298 |     376 B |
