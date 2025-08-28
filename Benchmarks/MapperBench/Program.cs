// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MapperBench;
using MapperBench.Supports;
using System.Linq.Expressions;


CustomerConvertBench convertBench = new();
convertBench.Setup();
//convertBench.Test();
//var auto = convertBench.BuildAuto();
var poco = convertBench.BuildPoco();
//var dto1 = convertBench.Auto();
//var dto2 = convertBench.Convert();
//var dto3 = convertBench.AutoFunc();
//convertBench.Customer.Name = "changed";
//var dto12 = convertBench.Auto();
//var dto22 = convertBench.Convert();
Console.ReadLine();
//BenchmarkRunner.Run<CustomerConvertBench>();
//UserConvertBench userConvertBench = new();
//userConvertBench.Setup();
//userConvertBench.BuildPoco();
//BenchmarkRunner.Run<UserConvertBench>();

//Expression<Action<Customer>> expression = static customer => customer.Name = "changed";
