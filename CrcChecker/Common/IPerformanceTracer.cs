namespace CrcChecker.Common
{
    public interface IPerformanceTracer
    {
        void Begin(PerformanceMarker performanceMarker, string param0 = null);
        void End(PerformanceMarker performanceMarker, string param0 = null);
    }
}