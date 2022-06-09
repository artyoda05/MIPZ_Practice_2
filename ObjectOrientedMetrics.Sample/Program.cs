// See https://aka.ms/new-console-template for more information

using System.Reflection;
using ObjectOrientedMetrics.Analyzers;
using ObjectOrientedMetrics.Analyzers.Assembly;
using ObjectOrientedMetrics.Analyzers.Listing;

IEnumerable<object>? GetAnalyzers(Type baseType, string constructorParameter)
{
    return Assembly.GetAssembly(typeof(Analyzer<>))
        .GetTypes()
        .Where(type => type.BaseType.Name.Equals(baseType.Name))
        .Select(type => type.GetConstructor(new[]
        {
            typeof(string),
        }))
        .Select(constructor => constructor.Invoke(new[] { constructorParameter }))
        .ToArray();
}

var filepath = "C:\\Users\\artyo\\source\\repos\\ObjectOrientedMetrics";
var repoName = "C:\\Users\\artyo\\source\\repos\\ObjectOrientedMetrics\\ObjectOrientedMetrics\\bin\\Debug\\net6.0\\ObjectOrientedMetrics.dll";

var analyzers = Enumerable.Union(GetAnalyzers(typeof(ListingAnalyzer<>), filepath),
    GetAnalyzers(typeof(AssemblyAnalyzer<>), repoName)).ToArray();

foreach (var analyzer in analyzers)
{
    Console.WriteLine(analyzer);
    Console.WriteLine();
}