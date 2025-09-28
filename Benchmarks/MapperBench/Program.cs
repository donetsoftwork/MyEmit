// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MapperBench;
using MapperBench.Supports;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

#region CustomerConvertBench
//CustomerConvertBench convertBench = new();
//convertBench.Setup();
////convertBench.Test();
//var auto = convertBench.BuildAuto();
//var autoFunc = convertBench.AutoFunc();
//var converter = convertBench.Converter();
//var pocoFunc = convertBench.PocoFunc();
//var poco = CustomerTests.BuildPoco();
#endregion

#region TreeBench
//TreeBench treeBench = new();
//treeBench.Setup();
////treeBench.PocoFunc();
//var auto = treeBench.BuildAuto();
//var poco = treeBench.BuildPoco();
//var invoke = treeBench.BuildInvoke();
#endregion
#region TreeBench2
//TreeBench2 treeBench2 = new();
//treeBench2.Setup();
////treeBench2.PocoFunc();
//var auto = treeBench2.BuildAuto();
//var poco = treeBench2.BuildPoco();
#endregion
#region UserConvertBench
//UserConvertBench userConvertBench = new();
//userConvertBench.Setup();
//userConvertBench.BuildPoco();
#endregion
#region NodeBench
//NodeBench nodeBench = new();
//nodeBench.Setup();
//var auto = nodeBench.BuildAuto();
//var poco = nodeBench.BuildPoco();
#endregion
#region MenuBench
//MenuBench menuBench = new();
//menuBench.Setup();
//var auto = menuBench.BuildAuto();
//var auto0 = menuBench.BuildAuto0();
//var poco = menuBench.BuildPoco();
//var poco0 = menuBench.BuildPoco0();
//var cache = menuBench.BuildCache();
#endregion
#region SoldierTeamBench
//SoldierTeamBench soldierTeamBench = new();
//soldierTeamBench.Setup();
//var auto = soldierTeamBench.BuildAuto();
////var poco = soldierTeamBench.BuildPoco();
////var noCache = soldierTeamBench.BuildCache();
//var context = soldierTeamBench.BuildContextFunc();
#endregion
//var dto5 = CustomerTests.BySys();
//var dto6 = CustomerTests.ByFast();
//var dto4 = CustomerTests.CustomerToDTO(CustomerConvertBench.Customer);
//Console.ReadLine();
//BenchmarkRunner.Run<CustomerConvertBench>();
//BenchmarkRunner.Run<UserConvertBench>();
//BenchmarkRunner.Run<ToDictionaryBench>();
//BenchmarkRunner.Run<TreeBench>();
//BenchmarkRunner.Run<TreeBench2>();
//BenchmarkRunner.Run<NodeBench>();
//BenchmarkRunner.Run<MenuBench>();
BenchmarkRunner.Run<SoldierTeamBench>();

