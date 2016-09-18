using log4net;

namespace CrcChecker.Common
{
    class Log4NetPerformanceTracer : IPerformanceTracer
    {
        private static readonly ILog _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Begin(PerformanceMarker performanceMarker, string param0 = null)
        {
            Log("BEGIN", performanceMarker, param0);
        }

        public void End(PerformanceMarker performanceMarker, string param0 = null)
        {
            Log("END", performanceMarker, param0);
        }

        private void Log(string traceType, PerformanceMarker performanceMarker, string param0 = null)
        {
            if (string.IsNullOrEmpty(param0))
            {
                _Logger.InfoFormat("{0}, {1}", traceType, performanceMarker.ToString());
            }
            else
            {
                _Logger.InfoFormat("{0}, {1}, {2}", traceType, performanceMarker.ToString(), param0);
            }
        }
    }
}
