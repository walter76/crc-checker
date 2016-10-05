using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FileIoPerformanceComparer.BinaryReaderMethods
{
    internal class Win32FileIOReader
    {
        private const uint GENERIC_READ = 0x80000000;
        private const uint OPEN_EXISTING = 3;

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe IntPtr CreateFile
        (
             string FileName,          // file name
             uint DesiredAccess,       // access mode
             uint ShareMode,           // share mode
             uint SecurityAttributes,  // Security Attributes
             uint CreationDisposition, // how to create
             uint FlagsAndAttributes,  // file attributes
             int hTemplateFile         // handle to template file
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile
        (
             IntPtr hFile,      // handle to file
             void* pBuffer,            // data buffer
             int NumberOfBytesToRead,  // number of bytes to read
             int* pNumberOfBytesRead,  // number of bytes read
             int Overlapped            // overlapped buffer which is used for async I/O.  Not used here.
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle
        (
             IntPtr hObject     // handle to object
        );

        IntPtr pHandle;

        public bool Open(string filename)
        {
            pHandle = CreateFile(filename, GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
            return (pHandle != IntPtr.Zero);
        }

        public unsafe int Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(pHandle, p + index, count, &n, 0))
                {
                    return 0;
                }
            }

            return n;
        }

        public bool Close()
        {
            return CloseHandle(pHandle);
        }

        public static unsafe byte[] ReadAllBytes(string filename)
        {
            long length = new FileInfo(filename).Length;
            var buffer = new byte[length];

            var fr = new Win32FileIOReader();
            if (fr.Open(filename))
            {
                long pos = 0L;
                while (pos < length)
                {
                    var chunk = new byte[512];
                    var bytesRead = fr.Read(chunk, 0, 512);
                    Array.Copy(chunk, 0L, buffer, pos, bytesRead);
                    pos += bytesRead;
                }

                fr.Close();
            }

            return buffer;
        }
    }
}
