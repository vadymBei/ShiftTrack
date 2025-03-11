namespace ShiftTrack.Client.Http.Configs;

public class TimeoutConfig
{
    public int TimeoutInSeconds { get; set; }

    protected internal void SetTimeout(int timeoutInSeconds)
    {
        TimeoutInSeconds = timeoutInSeconds;
    }
}