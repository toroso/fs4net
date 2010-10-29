using System;

namespace fs4net.Framework
{
    public class NullLogger : ILogger
    {
        public static ILogger Instance = new NullLogger();

        private NullLogger() { }

        public void LogSwallowedException(string message, Exception exception) { }
    }
}