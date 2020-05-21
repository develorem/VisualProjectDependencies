using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VisualProjectDependencies
{
    public interface IGraphRenderer
    {
        void Render(IEnumerable<ProjectGraph> projects);
    }

    public class FileGraphRenderer : IGraphRenderer
    {
        private readonly string _outputPath;

        public FileGraphRenderer(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void Render(IEnumerable<ProjectGraph> projects)
        {
            var text = GenerateProjectDependencyReport(projects);
            SaveFile(_outputPath, text);
        }

        private string GenerateProjectDependencyReport(IEnumerable<ProjectGraph> projectGraphs)
        {
            var sb = new StringBuilder();

            foreach (var project in projectGraphs.OrderBy(p => p.DependencyDepth))
            {
                sb.AppendLine($"{project.Project.Name} ({project.DependencyDepth})");
                foreach (var dependency in project.ProjectDependencies.OrderBy(p => p.DependencyDepth))
                {
                    sb.AppendLine($" - {dependency.Project.Name} [{dependency.DependencyDepth}]");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void SaveFile(string filename, string content)
        {
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, content);
        }
    }

    public class ConsoleGraphRenderer : IGraphRenderer
    {
        public void Render(IEnumerable<ProjectGraph> projects)
        {
            Console.WriteLine($"PROJECT GRAPH RESULT ({projects.Count()} projects)");
            Console.WriteLine("----------------------------------------");

            foreach (var project in projects.OrderBy(p => p.DependencyDepth))
            {
                Console.WriteLine($"  {project.Project.Name} [{project.DependencyDepth}]");
            }

            Console.WriteLine("----------------------------------------");
        }
    }
}
