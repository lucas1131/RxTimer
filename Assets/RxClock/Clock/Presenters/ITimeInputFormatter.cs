namespace RxClock.Clock
{
    public interface ITimeInputFormatter
    {
        (string format, int caretOffset) EditFormat(string text);
        string CommitFormat(string text);
    }
}