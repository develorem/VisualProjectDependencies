using System.IO;
using System.Reflection;

namespace VisualProjectDependencies.Tests
{
    public static class TestData
    {
        public static string LoadSampleSolutionText()
        {            
            return LoadResource(@"VisualProjectDependencies.Tests.Data.SampleSolution.txt");            
        }

        private static string LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();            

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

}