using System;
using System.Collections.Generic;

namespace fs4net.Framework.Impl
{
    internal class Validator
    {
        private readonly string _path;
        private Exception _exception;

        public Validator(string path)
        {
            _path = path;
        }

        public bool HasError
        {
            get { return _exception != null; }
        }

        public void Ensure(bool condition, Validator validator)
        {
            if (condition) return;
            AddError(validator);
        }

        public void Ensure<TException>(bool condition, string format, params object[] args)
            where TException: Exception
        {
            if (condition) return;
            AddError<TException>(format, args);
        }

        public void AddError(Validator validator)
        {
            if (_exception == null)
            {
                _exception = validator._exception;
            }
        }

        public void AddError<TException>(string format, params object[] args)
            where TException : Exception
        {
            if (_exception == null)
            {
                _exception = (Exception)Activator.CreateInstance(typeof(TException), string.Format(format, GetArgsWithPath(args)));
            }
        }

        private object[] GetArgsWithPath(IEnumerable<object> args)
        {
            var argsWithPath = new List<object> {_path};
            argsWithPath.AddRange(args);
            return argsWithPath.ToArray();
        }

        public void ThrowOnError()
        {
            if (_exception != null) throw _exception;
        }
    }
}