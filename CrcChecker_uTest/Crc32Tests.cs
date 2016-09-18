using CrcChecker;
using CrcChecker.Common;
using Moq;
using NUnit.Framework;

namespace CrcChecker_uTest
{
    [TestFixture]
    public class Crc32Tests
    {
        [TestCase(0x0, 3523407757)]
        [TestCase(0x1, 2768625435)]
        [TestCase(0xff, 4278190080)]
        public void ComputeChecksum_OneByte_CalculatesChecksum(byte input, uint expectedResult)
        {
            var crc32 = new Crc32(new NoTracingPerformanceTracer());
            Assert.That(crc32.ComputeChecksum(new byte[] { input }), Is.EqualTo(expectedResult));
        }

        [TestCase(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }, (uint) 1696784233)]
        [TestCase(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, (uint) 558161692)]
        [TestCase(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7 }, 2292869279)]
        [TestCase(new byte[] { 0x27, 0x11, 0x43, 0xa0, 0xfe, 0x23, 0x67, 0x98 }, (uint) 638171955)]
        public void ComputeChecksum_MultiByte_CalculatesChecksum(byte[] input, uint expectedResult)
        {
            var crc32 = new Crc32(new NoTracingPerformanceTracer());
            Assert.That(crc32.ComputeChecksum(input), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ComputeChecksum_PerformanceTrace_Called()
        {
            var performanceTracerMock = new Mock<IPerformanceTracer>();
            var crc32 = new Crc32(performanceTracerMock.Object);

            crc32.ComputeChecksum(new byte[] { 0x1 });

            performanceTracerMock.Verify(performanceTracer => performanceTracer.Begin(PerformanceMarker.ComputeChecksum, null), Times.Once);
            performanceTracerMock.Verify(performanceTracer => performanceTracer.End(PerformanceMarker.ComputeChecksum, null), Times.Once);
        }

        [Test]
        public void ComputeChecksum_PerformanceTrace_CalledWithFilename()
        {
            string filename = "file.dat";
            var performanceTracerMock = new Mock<IPerformanceTracer>();
            var crc32 = new Crc32(performanceTracerMock.Object);

            crc32.ComputeChecksum(new byte[] { 0x1 }, filename);

            performanceTracerMock.Verify(performanceTracer => performanceTracer.Begin(PerformanceMarker.ComputeChecksum, filename), Times.Once);
            performanceTracerMock.Verify(performanceTracer => performanceTracer.End(PerformanceMarker.ComputeChecksum, filename), Times.Once);
        }

        [Test]
        public void ComputeChecksumAsBytes_ConvertsToByteArray()
        {
            var crc32 = new Crc32(new NoTracingPerformanceTracer());
            byte[] currentResult = crc32.ComputeChecksumAsBytes(new byte[] { 0x1 });
            var expectedResult = new byte[] { 0x1B, 0xDF, 0x05, 0xA5 };

            Assert.That(currentResult, Is.EqualTo(expectedResult));
        }
    }
}
