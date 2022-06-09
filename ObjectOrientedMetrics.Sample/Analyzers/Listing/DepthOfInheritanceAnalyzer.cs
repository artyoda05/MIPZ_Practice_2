namespace ObjectOrientedMetrics.Analyzers.Listing;

public class DepthOfInheritanceAnalyzer : ListingAnalyzer<List<(string, int)>>
{
    public DepthOfInheritanceAnalyzer(string moduleListing) : base(moduleListing)
    {
    }

    public override List<(string, int)> Analyze()
    {
        List<(string, int)> result = new ();

        foreach (var className in AllClasses)
        {
            result.Add((className, GetDepthRecursive(className)));
        }

        return result.OrderByDescending(x => x.Item2).ToList();

    }

    private int GetDepthRecursive(string node)
    {
        var childList = ChildList(node);
        if (!childList.Any())
        {
            return 1;
        }

        var maxDepth = 0;

        foreach (var child in childList)
        {
            var depth = GetDepthRecursive(child.Item2) + 1;
            if (maxDepth < depth)
            {
                maxDepth = depth;
            }
        }

        return maxDepth;
    }

    public override string ToString()
    {
        var analyzed = Analyze();

        var str = new [] { "Depth Of Inheritance:" }.Union(analyzed.Select(a => $"    {a.Item1}: {a.Item2}"));

        return string.Join("\n", str);
    }
}