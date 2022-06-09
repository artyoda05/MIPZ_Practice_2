using System.Reflection;

namespace ObjectOrientedMetrics.Analyzers.Assembly;

public class MethodHidingFactorAnalyzer : AssemblyAnalyzer<double>
{
    public MethodHidingFactorAnalyzer(string repoName) : base(repoName)
    {
    }

    public override double Analyze()
    {
        var assembly = System.Reflection.Assembly.LoadFrom(RepoName);

        var privateMethods = 0;
        var allMethods = 0;

        foreach (var type in assembly.GetTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy))
            {
                if (!method.IsPublic && !method.IsPrivate) 
                    continue;
                
                allMethods++;

                if (method.IsPrivate)
                {
                    privateMethods++;
                }
            }
        }

        return (double)privateMethods / allMethods;
    }

    public override string ToString()
    {
        return $"Method Hiding Factor: {Analyze()}";
    }
}