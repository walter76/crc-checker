using System.IO;

namespace FileIoPerformanceComparer.BinaryReaderMethods
{
    internal class FileReadAllBytesFileReader
    {
        public static byte[] ReadFile(string filename)
        {
            return File.ReadAllBytes(filename);
        }
    }
}
