using System;

namespace fs4net.Framework
{
    /// <summary>
    /// The rooted path descriptors all have the possibility to log an instance of this interface (provided when
    /// creating the path descriptor through an IFileSystem factory method).
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Is called upon when fs4net swallows exceptions in methods with no-throw contract in occations where the
        /// exception can't be prevented.
        /// </summary>
        void LogSwallowedException(string message, Exception exception);
    }
}