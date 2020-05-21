using System;

namespace VisualProjectDependencies
{
    public class Project
    {
        public Guid ProjectTypeId { get; set; }

        public string Name { get; set; }
        public Guid Id { get; set; }
        public string SolutionRelativePath { get; set; }

        public string FullPath { get; set; }
    }
}
