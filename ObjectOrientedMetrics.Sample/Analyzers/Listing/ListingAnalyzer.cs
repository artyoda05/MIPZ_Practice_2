using System.Text.RegularExpressions;

namespace ObjectOrientedMetrics.Analyzers.Listing;

public abstract class ListingAnalyzer<T> : Analyzer<T>
{
    protected readonly List<(string, string)> ParentChildrenList;
    protected readonly List<string> AllClasses;

    protected ListingAnalyzer(string filepath)
    {
        var moduleListing = GetListing(filepath);

        AllClasses = ParseAllClasses(moduleListing);
        ParentChildrenList = ParseInheritedClasses(moduleListing);
    }

    public string GetListing(string filepath)
    {
        var files = Directory.GetFiles(filepath, "*.cs", SearchOption.AllDirectories);
        
        var allLinesInAllFiles = new List<string>();
        foreach (var filename in files)
        {
            allLinesInAllFiles.AddRange(File.ReadAllLines(filename).ToList());
        }

        return string.Join("\n", allLinesInAllFiles);
    }

    public IEnumerable<(string, string)> ChildList(string node)
    {
        return ParentChildrenList.Where(i => i.Item1 == node);
    }

    public List<string> ParseAllClasses(string moduleListing)
    {
        List<string> allClasses = GetAllMatches(moduleListing, "class\\s*[a-zA-Z]*\\s*[:{]");
        List<string> result = new List<string>();

        foreach (string inhClass in allClasses)
        {
            if (inhClass.Length < 8)
            {
                continue;
            }

            string className = inhClass.Substring(6); //getting rid of "class " 
            className = className.Remove(className.Length - 2); //getting rid of last char '{' or ':'
            className = className.Trim();

            if (!result.Contains(className))
            {
                result.Add(className);
            }
        }

        return result;
    }

    public List<(string, string)> ParseInheritedClasses(string moduleListing)
    {
        var inheritedClasses = GetAllMatches(moduleListing, "class\\s*[a-zA-Z]*\\s*:\\s*[a-zA-Z]*");
        var result = new List<(string, string)>();

        foreach (var inhClass in inheritedClasses)
        {
            if (inhClass.Length < 7)
            {
                continue;
            }

            var splitted = inhClass.Substring(6).Split(':');

            if (splitted.Length != 2)
            {
                continue;
            }

            var parent = splitted.Last().Trim();
            var children = splitted.First().Trim();

            if (AllClasses.Contains(parent))
            {
                result.Add((parent, children));
            }
        }

        return result;
    }

    public List<string> GetAllMatches(string moduleListing, string pattern)
    {
        var rgx = new Regex(pattern);

        var result = new List<string>();

        var firstMatch = rgx.Match(moduleListing);

        if (firstMatch.Success)
        {
            result.Add(firstMatch.Value);
            foreach (Match m in rgx.Matches(moduleListing, firstMatch.Index + firstMatch.Length))
            {
                result.Add(m.Value);
            }
        }

        return result;
    }
}