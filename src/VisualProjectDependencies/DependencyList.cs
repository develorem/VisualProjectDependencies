using System.Collections.Generic;

namespace VisualProjectDependencies
{
    public class DependencyList
    {
        public IEnumerable<string> ProjectDependencyPaths { get; set; }

        public DependencyList()
        {
            ProjectDependencyPaths = new List<string>();
        }
    }
}
