using System;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates
{
    public class AssertLogger : ILogger
    {
        public static ILogger Instance = new AssertLogger();

        private AssertLogger() { }
        public void LogSwallowedException(string message, Exception exception)
        {
            Assert.Fail(string.Format("{0}; Exception: {1}", message, exception));
        }
    }
}