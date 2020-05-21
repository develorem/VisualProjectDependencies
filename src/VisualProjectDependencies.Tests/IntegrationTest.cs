using NUnit.Framework;

namespace VisualProjectDependencies.Tests
{
    [TestFixture(Category = "Integration")]
    public class IntegrationTest
    { 
        [Test]        
        public void EndToEndTestSmallSolution()
        {
            // This is a real SLN file on my system.
            // Replace with one of your own

            var slnFile = @"C:\Me\dev\develorem\HomeAutomation\Home.Automation.sln";
            var outputFile = @"C:\Me\dev\temp\ProjectDependencies\Home.Automation.Dependencies.txt";

            Program.Main(new[] { slnFile, outputFile });

        }

    }
}