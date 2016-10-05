using System;
using System.IO;

namespace FileIoPerformanceComparer.BinaryReaderMethods
{
    internal class ChunkedFileReader
    {
        public static byte[] ReadFile(string filename)
        {
            long length = new FileInfo(filename).Length;
            var buffer = new byte[length];

            using (var binaryReader =
                new BinaryReader(
                    new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                long pos = 0L;
                while (pos < length)
                {
                    var chunk = binaryReader.ReadBytes(512);
                    Array.Copy(chunk, 0L, buffer, pos, chunk.LongLength);
                    pos += chunk.LongLength;
                }
            }

            return buffer;
        }
    }
}
