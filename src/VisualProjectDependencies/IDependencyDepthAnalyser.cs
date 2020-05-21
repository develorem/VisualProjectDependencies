using System.Collections.Generic;
using System.Linq;

namespace VisualProjectDependencies
{
    public interface IDependencyDepthAnalyser
    {
        int Analyse(ProjectGraph project);
    }

    public class DependencyDepthAnalyser : IDependencyDepthAnalyser
    {
        public int Analyse(ProjectGraph project)
        {
            // Although it shouldn't be the case, we will track projects we have seen anyway, on the off chance we end up in a loop
            var projectsSeen = new List<ProjectGraph>();
            var counter = AnalyseRecursive(project, 0, projectsSeen);
            return counter;
        }

        private int AnalyseRecursive(ProjectGraph project, int currentCount, List<ProjectGraph> projectsSeen)
        {
            if (project.ProjectDependencies.Count() == 0)
                return currentCount;
            
            var unseenChildren = project.ProjectDependencies.Where(x => !projectsSeen.Contains(x)).ToArray();
            if (unseenChildren.Length == 0)
                return currentCount;

            currentCount++;

            projectsSeen.AddRange(unseenChildren);

            var highestDepth = currentCount;
            foreach (var p in unseenChildren)
            {
                var depth = AnalyseRecursive(p, currentCount, projectsSeen);
                if (depth > highestDepth) highestDepth = depth;
            }
            return highestDepth;
        }
    }

}
