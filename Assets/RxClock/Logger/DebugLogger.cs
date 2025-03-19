using UnityEngine;

public class DebugLogger : ILogger
{
    public void Info(string msg) => Debug.Log(msg);
    public void Warning(string msg) => Debug.LogWarning(msg);
    public void Error(string msg) => Debug.LogError(msg);
}
