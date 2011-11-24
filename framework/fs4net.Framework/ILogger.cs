using System;

namespace fs4net.Framework
{
    /// <summary>
    /// </summary>
    public interface ILogger
    {
        void LogSwallowedException(string message, Exception exception);
    }
}