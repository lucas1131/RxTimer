namespace RxClock.Clock
{
    public struct TimerFinishedMessage
    {
        public enum Reason
        {
            Completed,
            Aborted,
            Unknown,
        }
        
        public Reason FinishReason;
        
        public TimerFinishedMessage(Reason reason)
        {
            FinishReason = reason;
        }
    }
}