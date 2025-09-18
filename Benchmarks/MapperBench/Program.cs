// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MapperBench;
using MapperBench.Supports;
using System.Linq.Expressions;


//CustomerConvertBench convertBench = new();
//convertBench.Setup();
//convertBench.Test();
//var auto = convertBench.BuildAuto();
//var poco = CustomerTests.BuildPoco();
//var dto1 = convertBench.Auto();
//var dto2 = convertBench.Poco();
//var dto3 = convertBench.AutoFunc();
//convertBench.Customer.Name = "changed";
//var dto12 = convertBench.Auto();
//var dto22 = convertBench.Convert();

//TreeBench treeBench = new();
//treeBench.Setup();
////treeBench.PocoFunc();
//var auto = treeBench.BuildAuto();
//var poco = treeBench.BuildPoco();

//UserConvertBench userConvertBench = new();
//userConvertBench.Setup();
//userConvertBench.BuildPoco();
//NodeTests nodeTest = new();
////nodeTest.AutoMap();
//nodeTest.BuildAuto();

//var dto5 = CustomerTests.BySys();
//var dto6 = CustomerTests.ByFast();
//var dto4 = CustomerTests.CustomerToDTO(CustomerConvertBench.Customer);
//Console.ReadLine();
//BenchmarkRunner.Run<CustomerConvertBench>();
//BenchmarkRunner.Run<UserConvertBench>();
//BenchmarkRunner.Run<ToDictionaryBench>();
BenchmarkRunner.Run<TreeBench>();


