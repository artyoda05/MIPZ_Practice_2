namespace ObjectOrientedMetrics.Analyzers.Listing;

public class NumberOfChildrenAnalyzer : ListingAnalyzer<List<(string, int)>>
{
    public NumberOfChildrenAnalyzer(string moduleListing) : base(moduleListing)
    {
    }

    public override List<(string, int)> Analyze()
    {
        var result = AllClasses
            .Select(className => (className, ChildList(className).Count()))
            .OrderByDescending(x => x.Item2)
            .ToList();

        return result;
    }

    public override string ToString()
    {
        var analyzed = Analyze();

        var str = new [] { "Number Of Children:" }.Union(analyzed.Select(a => $"    {a.Item1}: {a.Item2}"));

        return string.Join("\n", str);

        return $"Number Of Children: {Analyze()}";
    }
}