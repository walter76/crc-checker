using CrcChecker.Common;
using CrcChecker.Optimized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace CrcChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsage();
                return;
            }

            string directoryToCheck = args[0];
            IPerformanceTracer perfTrace = new NoTracingPerformanceTracer();
            if (args.Length > 1)
            {
                directoryToCheck = args[1];

                if (args[0].Equals("-p"))
                {
                    perfTrace = new Log4NetPerformanceTracer();
                }
                else if (args[0].Equals("-d"))
                {
                    PrintDirectorySummary(args[1]);
                    return;
                }
                else
                {
                    PrintUsage();
                    return;
                }
            }
            
            //var crcCalculator = new PrimitiveCrcCalculator();
            var crcCalculator = new CrcCalculator(perfTrace);
            crcCalculator.CheckFiles(CreateFlatListOfFilesToCheck(directoryToCheck), perfTrace);

            Console.WriteLine("Finished. Hint <Enter> to exit...");
            Console.ReadLine();
        }

        private static void PrintDirectorySummary(string rootDirectory)
        {
            var listOfFilesToCheck = CreateFlatListOfFilesToCheck(rootDirectory);
            foreach (var fileInfo in listOfFilesToCheck.Select(filename => new FileInfo(filename)))
            {
                Console.WriteLine("{0}, {1}", fileInfo.FullName, fileInfo.Length);
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: CrcChecker [-p|-d] <directory>");
        }

        private static IEnumerable<string> CreateFlatListOfFilesToCheck(string rootDirectory)
        {
            return Directory.EnumerateFiles(rootDirectory, "*.*", SearchOption.AllDirectories);
        }
    }
}
