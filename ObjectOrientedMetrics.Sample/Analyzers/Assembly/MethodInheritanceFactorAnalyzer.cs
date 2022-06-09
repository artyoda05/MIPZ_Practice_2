using System.Reflection;

namespace ObjectOrientedMetrics.Analyzers.Assembly;

public class MethodInheritanceFactorAnalyzer : AssemblyAnalyzer<double>
{
    public MethodInheritanceFactorAnalyzer(string moduleListing) : base(moduleListing)
    {
    }

    public override double Analyze()
    {
        var assembly = System.Reflection.Assembly.LoadFrom(RepoName);

        var inheritedOverridenMethods = 0;
        var allMethods = 0; //with all inherited


        foreach (var type in assembly.GetTypes())
        {
            allMethods += type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy).Length;

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy))
            {
                var m_base = method.GetBaseDefinition();

                if (m_base.ReflectedType.Name != method.ReflectedType.Name)
                {
                    inheritedOverridenMethods++;
                }
            }
        }

        var inheritedNotOverridenMethods = allMethods - inheritedOverridenMethods;

        return (double)inheritedNotOverridenMethods / allMethods;
    }

    public override string ToString()
    {
        return $"Method Inheritance Factor: {Analyze()}";
    }
}