using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VisualProjectDependencies
{
    public class Runner
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IProjectReader _projectReader;
        private readonly IDependencyDepthAnalyser _dependencyDepthAnalyser;
        private readonly IEnumerable<IGraphRenderer> _renderers;

        public Runner(ISolutionReader solutionReader, IProjectReader projectReader, IDependencyDepthAnalyser dependencyDepthAnalyser, IEnumerable<IGraphRenderer> renderers)
        {
            _solutionReader = solutionReader;
            _projectReader = projectReader;
            _dependencyDepthAnalyser = dependencyDepthAnalyser;
            _renderers = renderers;
        }

        public void Run(string solutionFile)
        {
            var basePath = Path.GetDirectoryName(solutionFile).TrimEnd('\\', '/');

            // 1. Open solution file
            var slnText = ReadFile(solutionFile);

            // 2. Parse it
            var projects = _solutionReader.ReadText(slnText).ToArray();

            // 3. Convert all projects to projectgraphs
            var projectGraphs = projects.Select(p => new ProjectGraph(p)).ToArray();

            // Get the project dependencies for each one
            ExtractProjectDependencies(basePath, projectGraphs);

            // Determine depth graph of each one
            UpdateDependencyDepths(projectGraphs);

            // Write to output
            foreach (var renderer in _renderers)
            {
                renderer.Render(projectGraphs);
            }
        }


        private void ExtractProjectDependencies(string basePath, IEnumerable<ProjectGraph> allProjectGraphs)
        {
            // Update all projects to get their full path
            foreach (var p in allProjectGraphs)
            {
                p.Project.FullPath = Path.Join(basePath, p.Project.SolutionRelativePath);
            }

            foreach (var projectGraph in allProjectGraphs)
            {                
                var projectText = ReadFile(projectGraph.Project.FullPath);
                var projectDependencyPaths = _projectReader.Read(projectText).ProjectDependencyPaths;

                // Find the right project graph based on the path output
                var currentProjectFolderOnly = Path.GetDirectoryName(projectGraph.Project.FullPath);

                var dependentGraphs = new List<ProjectGraph>();

                foreach (var dependencyPath in projectDependencyPaths) {
                    var destinationPath = Path.GetFullPath(Path.Join(currentProjectFolderOnly, dependencyPath)); // Handles the \..\ scenarios
                    var matchingProject = allProjectGraphs.FirstOrDefault(x => x.Project.FullPath.ToLower() == destinationPath.ToLower());

                    if (matchingProject == null) throw new ApplicationException("Expected a match");

                    dependentGraphs.Add(matchingProject);
                }

                projectGraph.ProjectDependencies = dependentGraphs;                
            }
        }

        private void UpdateDependencyDepths(IEnumerable<ProjectGraph> allProjectGraphs)
        {
            foreach (var project in allProjectGraphs)
            {
                var depth = _dependencyDepthAnalyser.Analyse(project);
                project.DependencyDepth = depth;
            }
        }

        private string ReadFile(string file)
        {
            using (var sr = new StreamReader(file))
            {
                return sr.ReadToEnd();
            }
        }
    
    }
}
