using System.Collections.Generic;
using System.Xml;

namespace VisualProjectDependencies
{
    public interface IProjectReader
    {
        DependencyList Read(string projectText);
    }

    public class ProjectReader : IProjectReader
    {
        public DependencyList Read(string projectText)
        {
            var dependencies = new List<string>();

            var xdoc = new XmlDocument();
            xdoc.LoadXml(projectText);
            var root = xdoc.DocumentElement;
            var nodes = root.SelectNodes("descendant::ProjectReference");
            
            for (var i=0;i<nodes.Count;i++)
            {
                var node = nodes[i];
                var path = node.Attributes["Include"].Value;
                dependencies.Add(path);
            }

            return new DependencyList { ProjectDependencyPaths = dependencies };
        }
    }

}
