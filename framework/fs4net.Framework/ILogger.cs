using System;

namespace fs4net.Framework
{
    public interface ILogger
    {
        void LogSwallowedException(string message, Exception exception);
    }
}