using NUnit.Framework;
using System.Linq;

namespace VisualProjectDependencies.Tests
{
    public class ProjectParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EnsureCanParseProjectFile()
        {
            var projectText = TestData.LoadSampleProjectText();
            var projectReader = new ProjectReader();

            var dependencies = projectReader.Read(projectText);

            Assert.AreEqual(2, dependencies.ProjectDependencyPaths.Count());

            Assert.AreEqual(@"..\Home.Common\Home.Common.csproj", dependencies.ProjectDependencyPaths.ElementAt(0));
        }
    }
}