using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FileIoPerformanceComparer
{
    internal class PinnedBufferChunkedFileReader
    {
        public static byte[] ReadFile(string filename)
        {
            long length = new FileInfo(filename).Length;
            var buffer = new byte[length];
            GCHandle bufferGcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

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

            bufferGcHandle.Free();

            return buffer;
        }
    }
}
