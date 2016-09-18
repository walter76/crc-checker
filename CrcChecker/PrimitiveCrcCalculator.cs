using CrcChecker.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrcChecker
{
    class PrimitiveCrcCalculator : ICrcCalculator
    {
        public void CheckFiles(IEnumerable<string> filesToCheck, IPerformanceTracer perfTrace)
        {
            perfTrace.Begin(PerformanceMarker.CheckFiles);

            var crc32 = new Crc32(perfTrace);

            foreach (var filename in filesToCheck)
            {
                var buffer = ChunkedFileReader.ReadFile(filename, perfTrace);
                var checksum = crc32.ComputeChecksum(buffer, filename);

                Console.WriteLine("{0}, {1}, 0x{2:X}", filename, new FileInfo(filename).Length, checksum);
            }

            perfTrace.End(PerformanceMarker.CheckFiles);
        }
    }
}
