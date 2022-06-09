using System.Reflection;

namespace ObjectOrientedMetrics.Analyzers.Assembly;

public class AttributeHidingFactorAnalyzer : AssemblyAnalyzer<double>
{
    public AttributeHidingFactorAnalyzer(string repoName) : base(repoName)
    {
    }

    public override double Analyze()
    {
        var assembly = System.Reflection.Assembly.LoadFrom(RepoName);

        var privateAttributes = 0;
        var allAttributes = 0;
        
        foreach (var type in assembly.GetTypes())
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy))
            {
                if (!field.IsPublic && !field.IsPrivate)
                    continue;

                allAttributes++;

                if (field.IsPrivate)
                {
                    privateAttributes++;
                }
            }
        }

        return (double)privateAttributes / allAttributes;
    }

    public override string ToString()
    {
        return $"Attribute Hiding Factor: {Analyze()}";
    }
}