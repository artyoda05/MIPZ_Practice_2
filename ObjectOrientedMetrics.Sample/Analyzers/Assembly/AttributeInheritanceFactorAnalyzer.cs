namespace ObjectOrientedMetrics.Analyzers.Assembly;

public class AttributeInheritanceFactorAnalyzer : AssemblyAnalyzer<double>
{
    public AttributeInheritanceFactorAnalyzer(string repoName) : base(repoName)
    {
    }

    public override double Analyze()
    {
        return 1;
    }

    public override string ToString()
    {
        return $"Attribute Inheritance Factor: {Analyze()}";
    }
}