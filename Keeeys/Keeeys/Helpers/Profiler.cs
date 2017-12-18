using System;
using System.Diagnostics;

namespace Keeeys.Common.Helpers
{
    public class Profiler : IDisposable
    {
        private Stopwatch stopwatch;
        private string name;

        public Profiler(string name)
        {
#if DEBUG
            stopwatch = new Stopwatch();
            stopwatch.Start();
            this.name = name;
#endif
        }

        public void Dispose()
        {
#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine(String.Format("PROFILING:: {0}: {1}", this.name, stopwatch.Elapsed));
#endif
        }
    }
}
