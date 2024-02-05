using UnityEngine;

public static class DebugEx
{
    public enum LogLevel
    {
        None,
        Error,
        Warning,
        All
    }

    public static LogLevel CurrentLogLevel = LogLevel.All;

    public static void Log(object message, Object context = null)
    {
#if UNITY_EDITOR
        if (CurrentLogLevel >= LogLevel.All)
        {
            Debug.Log(message, context);
        }
#endif
    }

    public static void LogWarning(object message, Object context = null)
    {
#if UNITY_EDITOR
        if (CurrentLogLevel >= LogLevel.Warning)
        {
            Debug.LogWarning(message, context);
        }
#endif
    }

    public static void LogError(object message, Object context = null)
    {
#if UNITY_EDITOR
        if (CurrentLogLevel >= LogLevel.Error)
        {
            Debug.LogError(message, context);
        }
#endif
    }
}