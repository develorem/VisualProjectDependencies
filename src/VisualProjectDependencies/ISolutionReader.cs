using System;
using System.Collections.Generic;
using System.IO;
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
                if (project != null) yield return project;
            }
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

            var remaining = projectRow.Substring(equals + 1).TrimStart().TrimEnd();
            var tokens = remaining.Split(",");
            if (tokens == null || tokens.Length != 3) return null;

            p.Name = ScrubString(tokens[0]);
            p.Path = ScrubString(tokens[1]);
            p.Id = Guid.Parse(ScrubString(tokens[2]));

            return p;
        }

        public string ScrubString(string s)
        {
            return s.Trim().Trim('"').Trim();
        }
    }

}
