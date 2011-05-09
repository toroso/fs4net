using System;
using NUnit.Framework;

namespace fs4net.TestTemplates
{
    public static class Should
    {
        /// <summary>Like NUnit Assert.Throws but with better error message.</summary>
        public static void Throw<TExpectedException>(Action action)
        {
            Throw<TExpectedException>(null, action, null);
        }

        /// <summary>Like NUnit Assert.Throws but with better error message.</summary>
        public static void Throw<TExpectedException>(Action action, string errorMessage)
        {
            Throw<TExpectedException>(null, action, errorMessage);
        }

        /// <summary>Like NUnit Assert.Throws but with better error message.</summary>
        public static void Throw<TExpectedException>(string expectedMessage, Action action)
        {
            Throw<TExpectedException>(expectedMessage, action, null);
        }

        /// <summary>Like NUnit Assert.Throws but with better error message.</summary>
        public static void Throw<TExpectedException>(string expectedMessage, Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(TExpectedException))
                {
                    FailDueToWrongException<TExpectedException>(ex, errorMessage);
                }
                if (expectedMessage != null && ex.Message != expectedMessage)
                {
                    FailDueToWrongMessage(ex.Message, expectedMessage, errorMessage);
                }
                return; // Success
            }
            FailDueToNoException<TExpectedException>(errorMessage);
        }

        private static void FailDueToNoException<TExpectedException>(string errorMessage)
        {
            FailDueToWrongException<TExpectedException>(null, errorMessage);
        }

        private static void FailDueToWrongException<TExpectedException>(Exception ex, string errorMessage)
        {
            var actual = ex != null ? ex.ToString() : "<null>";
            Assert.Fail(string.Format(
@"{0}Expected exception was not fired.
  Expected: {1}
    Actual: {2}", MakeDescription(errorMessage), typeof(TExpectedException), actual));
        }

        private static void FailDueToWrongMessage(string actual, string expected, string errorMessage)
        {
            Assert.Fail(string.Format(
@"{0}Exception contained wrong message.
  Expected: {1}
    Actual: {2}", MakeDescription(errorMessage), expected, actual));
        }

        /// <summary>Like NUnit Assert.DoesNotThrow but with better error message.</summary>
        public static void NotThrow(Action action)
        {
            NotThrow(action, null);
        }

        /// <summary>Like NUnit Assert.DoesNotThrow but with better error message.</summary>
        public static void NotThrow(Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format(@"{0}Unexpected exception caught: {1}", MakeDescription(errorMessage), ex));
            }
        }

        private static string MakeDescription(string errorMessage)
        {
            return errorMessage != null ? string.Format("{0}. ", errorMessage) : string.Empty;
        }
    }
}