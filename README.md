![Build status](https://ci.appveyor.com/api/projects/status/uce7en2777724aaj?svg=true) 

# MapReduct.Net
In memory Map Reduce pattern implementation in .NET

!This project is under development

Inspired by the [Hadoop](http://http://hadoop.apache.org/)


# Test Result
## Word count with 10000 lines
``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows 10.0.14393
Processor=Intel(R) Core(TM) i7-6700HQ CPU 2.60GHz, ProcessorCount=8
Frequency=2531252 Hz, Resolution=395.0614 ns, Timer=TSC
dotnet cli version=1.0.0
  [Host]     : .NET Core 4.6.25009.03, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.25009.03, 64bit RyuJIT


```
 |                                          Method |       Mean |    StdErr |    StdDev |     Median | Rank |
 |------------------------------------------------ |----------- |---------- |---------- |----------- |----- |
 | WordCountWithoutCombinerAutoNumOfMappersPerNode | 40.5753 ms | 0.1591 ms | 0.5955 ms | 40.6124 ms |    1 |
 |         WordCountWithoutCombiner2MappersPerNode | 41.4730 ms | 0.3929 ms | 1.6670 ms | 41.2759 ms |    2 |
 |         WordCountWithoutCombiner4MappersPerNode | 42.5474 ms | 0.4869 ms | 2.0656 ms | 42.3984 ms |    2 |
 |            WordCountWithCombiner2MappersPerNode | 43.6283 ms | 0.5897 ms | 3.4384 ms | 42.4156 ms |    2 |
 |        WordCountWithCombinerAutoNumberOfMappers | 45.9555 ms | 0.7056 ms | 6.9130 ms | 43.3155 ms |    3 |
 |                  WordCountWithoutUsingMapReduce | 57.6846 ms | 0.5647 ms | 2.7080 ms | 56.3294 ms |    4 |

 
 
## Word count with 40000 lines
 
 ``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows 10.0.14393
Processor=Intel(R) Core(TM) i7-6700HQ CPU 2.60GHz, ProcessorCount=8
Frequency=2531252 Hz, Resolution=395.0614 ns, Timer=TSC
dotnet cli version=1.0.0
  [Host]     : .NET Core 4.6.25009.03, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.25009.03, 64bit RyuJIT


```
 |                                          Method |        Mean |     StdDev | Rank |
 |------------------------------------------------ |------------ |----------- |----- |
 |         WordCountWithoutCombiner2MappersPerNode | 103.9832 ms |  2.6470 ms |    1 |
 |            WordCountWithCombiner2MappersPerNode | 104.2379 ms |  1.3473 ms |    1 |
 | WordCountWithoutCombinerAutoNumOfMappersPerNode | 104.2735 ms |  2.7285 ms |    1 |
 |        WordCountWithCombinerAutoNumberOfMappers | 106.4188 ms |  4.0604 ms |    1 |
 |                  WordCountWithoutUsingMapReduce | 262.2894 ms | 11.2371 ms |    2 |
