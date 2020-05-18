using NUnit.Framework;
using System;
using System.Linq;

namespace VisualProjectDependencies.Tests
{
    public class SolutionParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EnsureCanParseSolutionFile()
        {
            var slnText = TestData.LoadSampleSolutionText();
            var solutionReader = new SolutionReader();

            var projects = solutionReader.ReadText(slnText);

            Assert.AreEqual(5, projects.Count());
        }

        [Test]
        public void EnsureParserCanPullOutAllProjectRows()
        {
            var slnText = TestData.LoadSampleSolutionText();
            var solutionReader = new SolutionReader();

            var projects = solutionReader.ParseOutProjects(slnText);
            Assert.AreEqual(5, projects.Count());
            
            foreach (var project in projects)
            {
                Assert.IsTrue(project.StartsWith("Project"));
            }
        }

        [Test]
        public void EnsureParserCanConvertProject()
        {
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Home.Data", "Home.Data\Home.Data.csproj", "{5FA136EB-B352-4087-B5C1-A93B3EFD853D}"
            var testProject = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"Home.Data\", \"Home.Data\\Home.Data.csproj\", \"{5FA136EB-B352-4087-B5C1-A93B3EFD853D}\"";
            var solutionReader = new SolutionReader();
            var project = solutionReader.ConvertToProject(testProject);

            Assert.AreEqual("Home.Data\\Home.Data.csproj", project.Path);
            Assert.AreEqual("Home.Data", project.Name);
            Assert.AreEqual("5FA136EB-B352-4087-B5C1-A93B3EFD853D", project.Id.ToString().ToUpper());
        }

        [Test]
        public void GuidParseTest()
        {
            var id = "{5FA136EB-B352-4087-B5C1-A93B3EFD853D}";
            var guid = Guid.Parse(id);

            Assert.AreEqual("5FA136EB-B352-4087-B5C1-A93B3EFD853D", guid.ToString().ToUpper());
        }
    }

}