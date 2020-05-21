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
            AnalyseRecursive(project);
            
            return project.DependencyDepth.Value;
        }

        private void AnalyseRecursive(ProjectGraph project)
        {
            // The tactic here is to find the bottom most child, set its depth to 0.
            // Then as we unwind the call stack, set the depth of the current project to the highsest child depth + 1

            if (project.ProjectDependencies.Count() == 0)
            {
                project.DependencyDepth = 0;
                return;
            }

            foreach (var child in project.ProjectDependencies)
            {
                if (child.DependencyDepth.HasValue) continue;

                AnalyseRecursive(child);
            }

            var maxChild = project.ProjectDependencies.Max(x => x.DependencyDepth);

            project.DependencyDepth = maxChild + 1;
        }

    }

}
