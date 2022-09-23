using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

//DOTNET_JitDisasm="$Main";DOTNET_JitDisasmSummary=1;DOTNET_TC_QuickJitForLoops=1;DOTNET_TieredPGO=1
var config = DefaultConfig.Instance
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddDiagnoser(new DisassemblyDiagnoser(new()))
    .HideColumns(Column.Error, Column.StdDev, Column.Median, Column.RatioSD)
    // .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60).WithEnvironmentVariables(
    // new EnvironmentVariable("DOTNET_TC_QuickJitForLoops", "0")
    // ))
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60).WithEnvironmentVariables(
        // new EnvironmentVariable("DOTNET_TC_QuickJitForLoops", "1")
    ))
    // .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70).WithEnvironmentVariables(
    //     new EnvironmentVariable("DOTNET_TC_QuickJitForLoops", "0")
    // ))
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70).WithEnvironmentVariables(
        //new EnvironmentVariable("DOTNET_TC_QuickJitForLoops", "1")
    ));

BenchmarkSwitcher
     .FromAssembly(typeof(Program).Assembly)
       .Run(args, config);