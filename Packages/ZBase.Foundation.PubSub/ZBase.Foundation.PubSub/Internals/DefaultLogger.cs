#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_LOGGING__
#endif

using System;

namespace ZBase.Foundation.PubSub.Internals
{
    internal class DefaultLogger : ILogger
    {
#if __ZBASE_FOUNDATION_PUBSUB_NO_LOGGING__

        public void LogException(Exception exception) { }

        public void LogError(string message) { }

        public void LogWarning(string message) { }

#else

        public void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        public void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

#endif
    }
}
