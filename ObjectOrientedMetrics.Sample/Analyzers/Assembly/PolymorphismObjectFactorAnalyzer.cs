using System.Reflection;

namespace ObjectOrientedMetrics.Analyzers.Assembly;

public class PolymorphismObjectFactorAnalyzer : AssemblyAnalyzer<double>
{
    public PolymorphismObjectFactorAnalyzer(string moduleListing) : base(moduleListing)
    {
    }

    public override double Analyze()
    {
        var assembly = System.Reflection.Assembly.LoadFrom(RepoName);

        var inheritedOverridenMethods = 0;
        var denominator = 0;

        foreach (var type in assembly.GetTypes())
        {
            var child = assembly
                .GetTypes()
                .Count(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            var newMethods = 0;

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy))
            {
                var m_base = method.GetBaseDefinition();

                if (m_base.ReflectedType.Name != method.ReflectedType.Name)
                {
                    inheritedOverridenMethods++;
                }
                else
                {
                    newMethods++;
                }
            }

            denominator += newMethods * child;
        }

        if (denominator == 0)
            return 0;

        return (double)inheritedOverridenMethods / denominator;
    }

    public override string ToString()
    {
        return $"Polymorphism Object Factor: {Analyze()}";
    }
}