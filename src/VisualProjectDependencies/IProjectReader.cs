namespace VisualProjectDependencies
{
    public interface IProjectReader
    {
        DependencyList Read(string projectPath);
    }
}
