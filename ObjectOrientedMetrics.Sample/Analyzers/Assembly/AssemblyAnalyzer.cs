namespace ObjectOrientedMetrics.Analyzers.Assembly;

public abstract class AssemblyAnalyzer<T> : Analyzer<T>
{
    protected string RepoName;

    protected AssemblyAnalyzer(string repoName)
    {
        RepoName = repoName;
    }
}