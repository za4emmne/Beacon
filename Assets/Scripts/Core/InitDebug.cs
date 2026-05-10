using UnityEngine;

public static class InitDebug
{
    public static bool Enabled = true;
    
    public static void Log(string message, Object context = null)
    {
        if (!Enabled) return;
        Debug.Log(message, context);
    }
    
    public static void LogWarning(string message, Object context = null)
    {
        if (!Enabled) return;
        Debug.LogWarning(message, context);
    }
    
    public static void LogError(string message, Object context = null)
    {
        if (!Enabled) return;
        Debug.LogError(message, context);
    }
}