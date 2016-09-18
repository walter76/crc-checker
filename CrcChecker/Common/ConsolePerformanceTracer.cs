using System;
using System.Globalization;

namespace CrcChecker.Common
{
    class ConsolePerformanceTracer : IPerformanceTracer
    {
        public void Begin(PerformanceMarker performanceMarker, string param0 = null)
        {
            Log("BEGIN", performanceMarker, param0);
        }

        public void End(PerformanceMarker performanceMarker, string param0 = null)
        {
            Log("END", performanceMarker, param0);
        }

        private string Timestamp
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }

        private void Log(string traceType, PerformanceMarker performanceMarker, string param0 = null)
        {
            if (string.IsNullOrEmpty(param0))
            {
                Console.WriteLine("{0}, {1}, {2}", Timestamp, traceType, performanceMarker.ToString());
            }
            else
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", Timestamp, traceType, performanceMarker.ToString(), param0);
            }
        }
    }
}
