using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace VisualProjectDependencies.Tests
{
    public static class TestData
    {
        public static string LoadSampleSolutionText()
        {            
            return LoadResource(@"VisualProjectDependencies.Tests.Data.SampleSolution.txt");
        }

        public static string LoadSampleProjectText()
        {
            return LoadResource(@"VisualProjectDependencies.Tests.Data.SampleProject.txt");
        }

        private static string LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();            

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    
        public static ProjectGraph GetNestedProjectGraphFourDeep()
        {
            // Deepest Graph:
            // Root => 3 children => 4 children => 2 child => 3 children            

            var project = CreateProjectGraphWithChildren(3);

            // Second child will have 2 children with 1 grand child, this is not deepest though
            var child1 = project.ProjectDependencies.ElementAt(1);
            AddChildren(child1, 2);
            var child10 = child1.ProjectDependencies.ElementAt(0);
            AddChildren(child10, 1);

            // Third child will have 4 children with 2 grand kids with 3 great grand kids
            var child2 = project.ProjectDependencies.ElementAt(2);
            AddChildren(child2, 4);
            var child23 = child2.ProjectDependencies.ElementAt(3);
            AddChildren(child23, 2);
            var child230 = child23.ProjectDependencies.ElementAt(0);
            AddChildren(child230, 3);

            return project;
        }

        private static ProjectGraph CreateProjectGraphWithChildren(int numberChildren)
        {
            var project = new ProjectGraph(new Project());
            AddChildren(project, numberChildren);
            return project;
        }

        private static void AddChildren(ProjectGraph project, int numberChildren)
        {
            var children = new List<ProjectGraph>();

            for (var i = 0; i < numberChildren; i++)
            {
                var child = new ProjectGraph(new Project());
                children.Add(child);
            }

            project.ProjectDependencies = children;
        }
        

    }

}