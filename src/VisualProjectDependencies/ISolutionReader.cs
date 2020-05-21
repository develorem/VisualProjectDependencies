using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VisualProjectDependencies
{
    public interface ISolutionReader
    {
        IEnumerable<Project> ReadFile(string solutionPath);

        IEnumerable<Project> ReadText(string solutionText);

    }

    public class SolutionReader : ISolutionReader
    {
        public IEnumerable<Project> ReadFile(string solutionPath)
        {
            using (var sr = new StreamReader(solutionPath))
            {
                var text = sr.ReadToEndAsync().Result;
                return ReadText(text);
            }
        }

        public IEnumerable<Project> ReadText(string solutionText)
        {
            var projectLines = ParseOutProjects(solutionText);
            foreach (var line in projectLines)
            {
                var project = ConvertToProject(line);
                if (project != null && IsProject(project)) yield return project;
            }
        }

        private bool IsProject(Project project)
        {
            // Some Project guids are in fact folders
            var folderGuids = new string[]
            {
                "2150E333-8FDC-42A3-9474-1A3956D46DE8",
                "66A26720-8FB5-11D2-AA7E-00C04F688DDE",
                "620A7797-1AB9-4396-AB77-2B686D949DDC"
            };

            if (folderGuids.Contains(project.ProjectTypeId.ToString().ToUpper()))
            {
                return false;
            }
            return true;
        }

        public IEnumerable<string> ParseOutProjects(string solutionText)
        {
            var rows = solutionText.Split(Environment.NewLine);
            foreach (var row in rows)
            {
                if (row.StartsWith("Project")) yield return row;
            }
        }

        public Project ConvertToProject(string projectRow)
        {
            if (!projectRow.StartsWith("Project")) return null;

            var p = new Project();

            // Example row
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Home.Data", "Home.Data\Home.Data.csproj", "{5FA136EB-B352-4087-B5C1-A93B3EFD853D}"
            // Tokens:
            // Project("?") = Name, Path, Id

            var equals = projectRow.IndexOf("=");
            if (equals <= -1) return null;

            var initial = projectRow.Substring(0, equals);
            var projectType = initial.Replace("Project(", "").Replace("\"", "").Replace(")", "").Trim().Replace("=", "").Trim();
            var projectGuid = Guid.Parse(projectType);
            
            var remaining = projectRow.Substring(equals + 1).TrimStart().TrimEnd();
            var tokens = remaining.Split(",");
            if (tokens == null || tokens.Length != 3) return null;

            p.ProjectTypeId = projectGuid;
            p.Name = ScrubString(tokens[0]);
            p.SolutionRelativePath = ScrubString(tokens[1]);
            p.Id = Guid.Parse(ScrubString(tokens[2]));

            return p;
        }

        public string ScrubString(string s)
        {
            return s.Trim().Trim('"').Trim();
        }
    }

}
