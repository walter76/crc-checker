using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CrcChecker.Optimized
{
    class CrcCalculatorJob
    {
        private readonly BlockingCollection<CrcWork> _CrcWorkPool;
        private readonly Crc32 _Crc32;
        private CancellationToken _CancellationToken;
        private Task _Task;

        public CrcCalculatorJob(BlockingCollection<CrcWork> crcWorkPool, Crc32 crc32)
        {
            _CrcWorkPool = crcWorkPool;
            _Crc32 = crc32;
        }

        public void Start(CancellationToken cancellationToken)
        {
            _CancellationToken = cancellationToken;

            _Task = Task.Run(() => Calculate());
        }

        private void Calculate()
        {
            bool checkForWork = true;

            while (checkForWork)
            {
                try
                {
                    var crcWork = _CrcWorkPool.Take(_CancellationToken);
                    var checksum = _Crc32.ComputeChecksum(crcWork.Buffer, crcWork.Filename);
                    crcWork.Buffer = null; // free the memory

                    Console.WriteLine("{0}, {1}, 0x{2:X}", crcWork.Filename, new FileInfo(crcWork.Filename).Length, checksum);
                }
                catch (OperationCanceledException)
                {
                    checkForWork = false;
                }
            }
        }
    }
}
