using System;
using System.Collections.Generic;

namespace VisualProjectDependencies
{
    public class ProjectGraph 
    {
        /// <summary>
        /// Other projects that this project depends on and needs to be able to build
        /// </summary>
        public IEnumerable<ProjectGraph> ProjectDependencies { get; set; }

        /// <summary>
        /// How many levels of dependency deep it is; 0 means no dependencies
        /// </summary>
        public int? DependencyDepth { get; set; }

        /// <summary>
        /// The project specific details as read from the solution file
        /// </summary>
        public Project Project { get; }

        public ProjectGraph(Project project)
        {
            Project = project;
            ProjectDependencies = Array.Empty<ProjectGraph>();
        }
    }

}
