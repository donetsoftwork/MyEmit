using BenchmarkDotNet.Running;
using PocoEmitBench;

//new ObjectPropertyGetBench().Poco();
//BenchmarkRunner.Run<ObjectPropertyGetBench>();
//BenchmarkRunner.Run<StringPropertyGetBench>();
//BenchmarkRunner.Run<IntPropertyGetBench>();
//new ObjectPropertySetBench().IL();
//BenchmarkRunner.Run<ObjectPropertySetBench>();
//new IntPropertySetBench().IL();
//BenchmarkRunner.Run<IntPropertySetBench>();
BenchmarkRunner.Run<StringPropertySetBench>();
