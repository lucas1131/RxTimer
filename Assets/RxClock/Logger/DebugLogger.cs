using UnityEngine;

public class DebugLogger : MonoBehaviour, ILogger
{
    public void Info(string msg) => Debug.Log(msg);
    public void Warning(string msg) => Debug.LogWarning(msg);
    public void Error(string msg) => Debug.LogError(msg);
}
