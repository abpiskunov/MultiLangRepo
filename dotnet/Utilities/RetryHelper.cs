using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public static class RetryHelper
    {
        public static T Execute<T>(Func<T> action, int maxRetries = 3, int delayMilliseconds = 1000)
        {
            Exception lastException = null;

            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (attempt < maxRetries)
                    {
                        int delay = delayMilliseconds * (int)Math.Pow(2, attempt);
                        Thread.Sleep(delay);
                    }
                }
            }

            throw new AggregateException(
                string.Format("Action failed after {0} retries", maxRetries + 1),
                lastException);
        }

        public static void Execute(Action action, int maxRetries = 3, int delayMilliseconds = 1000)
        {
            Execute<object>(() => { action(); return null; }, maxRetries, delayMilliseconds);
        }
    }
}
