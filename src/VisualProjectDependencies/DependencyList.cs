using System.Collections.Generic;

namespace VisualProjectDependencies
{
    public class DependencyList
    {
        IEnumerable<Project> ProjectDependencies { get; set; }
    }
}
