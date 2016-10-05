using System.IO;
using System.Runtime.InteropServices;

namespace FileIoPerformanceComparer
{
    internal class FileStreamFileReader
    {
        public static byte[] ReadFile(string filename)
        {
            long length = new FileInfo(filename).Length;
            var buffer = new byte[length];
            GCHandle bufferGcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                int pos = 0;
                while (pos < length)
                {
                    pos += fileStream.Read(buffer, pos, buffer.Length - pos);
                }
            }

            bufferGcHandle.Free();

            return buffer;
        }
    }
}
