namespace RxClock.Clock
{
    public interface ITimerInputFormatter
    {
        (string format, int caretOffset) EditFormat(string text);
        string CommitFormat(string text);
    }
}