using CrcChecker.Common;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CrcChecker.Optimized
{

    class CrcCalculator : ICrcCalculator
    {
        private readonly BlockingCollection<CrcWork> _CrcWorkPool = new BlockingCollection<CrcWork>();
        private readonly Crc32 _Crc32;

        public CrcCalculator(IPerformanceTracer perfTrace)
        {
            _Crc32 = new Crc32(perfTrace);
        }

        public void CheckFiles(IEnumerable<string> filesToCheck, IPerformanceTracer perfTrace)
        {
            perfTrace.Begin(PerformanceMarker.CheckFiles);

            var cancellationTokenSource = new CancellationTokenSource();

            CreateAndStartCalculatorJobs(cancellationTokenSource);

            AddWork(filesToCheck, perfTrace);
            WaitForWorkFinished();

            StopCalculatorJobs(cancellationTokenSource);

            perfTrace.End(PerformanceMarker.CheckFiles);
        }

        private void CreateAndStartCalculatorJobs(CancellationTokenSource cancellationTokenSource)
        {
            var cancellationToken = cancellationTokenSource.Token;

            for (int i = 0; i < 2; i++)
            {
                var crcCalculatorJob = new CrcCalculatorJob(_CrcWorkPool, _Crc32);
                crcCalculatorJob.Start(cancellationToken);
            }
        }

        private void AddWork(IEnumerable<string> filesToCheck, IPerformanceTracer perfTrace)
        {
            foreach (var filename in filesToCheck)
            {
                ReadFileAndAddToPool(filename, perfTrace);
            }
        }

        private void ReadFileAndAddToPool(string filename, IPerformanceTracer perfTrace)
        {
            var buffer = ChunkedFileReader.ReadFile(filename, perfTrace);
            _CrcWorkPool.Add(
                new CrcWork
                {
                    Filename = filename,
                    Buffer = buffer
                });
        }

        private void WaitForWorkFinished()
        {
            while (_CrcWorkPool.Count > 0) Thread.Sleep(100); // only check every 100 ms to let other threads breath
        }

        private static void StopCalculatorJobs(CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
