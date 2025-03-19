namespace RxClock.Clock
{
    public interface ITimeInputFormatter
    {
        string EditFormat(string text);
        string CommitFormat(string text);
    }
}