using System;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a logger that does nothing.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// A logger that does nothing.
        /// </summary>
        public static ILogger Instance = new NullLogger();

        private NullLogger() { }

        public void LogSwallowedException(string message, Exception exception) { }
    }
}