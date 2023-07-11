namespace ZBase.Foundation.PubSub
{
    public interface ILogger
    {
        void LogWarning(string message)
        {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_ZBASE_PUBSUB_DEBUG
            UnityEngine.Debug.LogWarning(message);
#endif
        }

        void LogError(string message)
        {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_ZBASE_PUBSUB_DEBUG
            UnityEngine.Debug.LogError(message);
#endif
        }

        void LogException(System.Exception exception)
        {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_ZBASE_PUBSUB_DEBUG
            UnityEngine.Debug.LogException(exception);
#endif
        }
    }
}
