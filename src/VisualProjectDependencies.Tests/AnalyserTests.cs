using NUnit.Framework;

namespace VisualProjectDependencies.Tests
{
    public class AnalyserTests
    {
        [Test]
        public void EnsureDepthCountIsAccurate()
        {
            var projectGraph = TestData.GetNestedProjectGraphFourDeep();
            var analyser = new DependencyDepthAnalyser();

            var depth = analyser.Analyse(projectGraph);

            Assert.AreEqual(4, depth);
        }
    }
}