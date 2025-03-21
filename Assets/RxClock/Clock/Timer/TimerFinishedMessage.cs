namespace RxClock.Clock
{
    public struct TimerFinishedMessage
    {
        public enum Reason
        {
            Completed,
            
            // this is unused here because there really is no reason in a simple app such as this but
            // if I was integrating this with a more complex app, there is a good chance that I would
            // want a way to cancel a timer externally, like with async tasks, cancellation tokens.
            Aborted, 
            
            Unknown
        }

        public Reason FinishReason;

        public TimerFinishedMessage(Reason reason)
        {
            FinishReason = reason;
        }
    }
}