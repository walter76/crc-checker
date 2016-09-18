using CrcChecker.Common;
using System;

namespace CrcChecker
{
    public class Crc32
    {
        private readonly IPerformanceTracer _PerfTrace;
        private readonly uint[] _Table;

        public uint ComputeChecksum(byte[] byteArray, string filename = null)
        {
            _PerfTrace.Begin(PerformanceMarker.ComputeChecksum, filename);

            uint crc = 0xffffffff;
            foreach (byte byteArrayEntry in byteArray)
            {
                var index = (byte)(((crc) & 0xff) ^ byteArrayEntry);
                crc = (crc >> 8) ^ _Table[index];
            }

            _PerfTrace.End(PerformanceMarker.ComputeChecksum, filename);

            return ~crc;
        }

        public byte[] ComputeChecksumAsBytes(byte[] byteArray) => BitConverter.GetBytes(ComputeChecksum(byteArray));

        public Crc32(IPerformanceTracer perfTrace)
        {
            _PerfTrace = perfTrace;

            _Table = new uint[256];
            FillLookupTable(_Table);
        }

        private static void FillLookupTable(uint[] lookupTable)
        {
            uint poly = 0xedb88320;

            uint temp = 0;
            for (uint i = 0; i < lookupTable.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (temp >> 1) ^ poly;
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                lookupTable[i] = temp;
            }
        }
    }
}
