using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PerfLogAnalyzer
{
    class CrcPerformance
    {
        public string File { get; set; }
        public DateTime FileReadBegin { get; set; }
        public DateTime FileReadEnd { get; set; }
        public DateTime ComputeChecksumBegin { get; set; }
        public DateTime ComputeChecksumEnd { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: PerfLogAnalyzer <filename>");
                return;
            }

            DateTime checkFilesBegin = DateTime.Now;
            DateTime checkFilesEnd = DateTime.Now;
            var crcPerformanceDictionary = new Dictionary<string, CrcPerformance>();

            using (var reader = File.OpenText(args[0]))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    var timestampSubstring = line.Substring(0, 23);
                    var timestamp = DateTime.ParseExact(timestampSubstring, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var performanceText = line.Substring(78);
                    var tokens = performanceText.Split(',');
                    var beginOrEnd = tokens[0].Trim();
                    var performanceMarker = tokens[1].Trim();

                    if ("CheckFiles" == performanceMarker)
                    {
                        if ("BEGIN" == beginOrEnd)
                        {
                            checkFilesBegin = timestamp;
                        }
                        if ("END" == beginOrEnd)
                        {
                            checkFilesEnd = timestamp;
                        }
                    }

                    if ("FileRead" == performanceMarker)
                    {
                        var file = tokens[2].Trim();
                        if ("BEGIN" == beginOrEnd)
                        {
                            crcPerformanceDictionary.Add(file, new CrcPerformance
                            {
                                File = file,
                                FileReadBegin = timestamp
                            });
                        }
                        if ("END" == beginOrEnd)
                        {
                            var crcPerformance = crcPerformanceDictionary[file];
                            crcPerformance.FileReadEnd = timestamp;
                        }
                    }

                    if ("ComputeChecksum" == performanceMarker)
                    {
                        var file = tokens[2].Trim();
                        if ("BEGIN" == beginOrEnd)
                        {
                            var crcPerformance = crcPerformanceDictionary[file];
                            crcPerformance.ComputeChecksumBegin = timestamp;
                        }
                        if ("END" == beginOrEnd)
                        {
                            var crcPerformance = crcPerformanceDictionary[file];
                            crcPerformance.ComputeChecksumEnd = timestamp;
                        }
                    }
                }
            }

            foreach(var crcPerformance in crcPerformanceDictionary.Values)
            {
                var fileReadInMs = crcPerformance.FileReadEnd.Subtract(crcPerformance.FileReadBegin).TotalMilliseconds;
                var computeCheckSumInMs = crcPerformance.ComputeChecksumEnd.Subtract(crcPerformance.ComputeChecksumBegin).TotalMilliseconds;
                Console.WriteLine("{0}, {1}, {2}", crcPerformance.File, fileReadInMs, computeCheckSumInMs);
            }

            var totalInMs = checkFilesEnd.Subtract(checkFilesBegin).TotalMilliseconds;
            Console.WriteLine("Total Time (ms): {0}", totalInMs);
        }
    }
}
