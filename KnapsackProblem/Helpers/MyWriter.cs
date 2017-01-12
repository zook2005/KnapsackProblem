using System;
using System.Diagnostics;
using System.IO;

namespace KnapsackProblem.Helpers
{
    public class MyWriter : IDisposable
    {
        private string reportPath;
        private readonly StreamWriter _sw;

        public MyWriter(string reportPath)
        {
            this.reportPath = reportPath;
            _sw = File.AppendText(reportPath);

        }

        public void WriteLine(string line = "")
        {
            _sw.WriteLine(line);
            Trace.WriteLine(line);
        }

        public void Dispose()
        {
            _sw.Dispose();
        }
    }
}