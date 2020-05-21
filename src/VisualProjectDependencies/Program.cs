using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;

namespace VisualProjectDependencies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expected two arguments: [solutionPath] [outputPath]");
                return;
            }

            var solutionPath = args[0];
            var outputPath = args[1];

            if (File.Exists(solutionPath) == false)
            {
                Console.WriteLine($"Supplied solution path does not exist: [{solutionPath}]");
                return;
            }

            var basePath = Path.GetDirectoryName(outputPath);
            if (Directory.Exists(basePath) == false)
            {
                Console.WriteLine($"Output path folder does not exist, please create the folder structure first: [{basePath}]");
                return;
            }

            
            var runner = GetRunner(outputPath);

            Console.WriteLine("Commencing analysis:");
            Console.WriteLine("  Solution File: " + solutionPath);
            Console.WriteLine("  Output File: " + outputPath);
            Console.WriteLine();

            var sw = new Stopwatch();
            sw.Start();
            
            runner.Run(solutionPath);

            sw.Stop();
            
            Console.WriteLine();
            Console.WriteLine($"Completed analysis in {sw.Elapsed.Seconds} seconds");
        }   
        
        static Runner GetRunner(string outputPath)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IGraphRenderer, FileGraphRenderer>(p => new FileGraphRenderer(outputPath));
            services.AddSingleton<IGraphRenderer, ConsoleGraphRenderer>();
            services.AddSingleton<IDependencyDepthAnalyser, DependencyDepthAnalyser>();
            services.AddSingleton<IProjectReader, ProjectReader>();
            services.AddSingleton<ISolutionReader, SolutionReader>();
            services.AddSingleton<Runner>();
            var provider = services.BuildServiceProvider();
            
            return provider.GetRequiredService<Runner>();
        }
    }
}
