using System.Collections.Generic;

namespace CrcChecker.Common
{
    interface ICrcCalculator
    {
        void CheckFiles(IEnumerable<string> filesToCheck, IPerformanceTracer perfTrace);
    }
}
