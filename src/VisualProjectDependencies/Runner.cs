using System;
using System.Collections.Generic;
using System.Text;

namespace VisualProjectDependencies
{
    public class Runner
    {
        private readonly ISolutionReader _solutionReader;

        public Runner(ISolutionReader solutionReader)
        {
            _solutionReader = solutionReader;
        }

        public void Run(string solutionFile, string outputFileName)
        {

        }
    }

    public class GraphGenerator
    { 

    }

    public class ProjectFileParser
    {
        public IEnumerable<Project> GetDependencies(string projectText)
        {
            // Todo
            throw new NotImplementedException();
        }
    }
}
