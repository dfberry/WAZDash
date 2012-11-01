


namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;

    // Summary:
    //     Provides data for the System.Timers.Timer.Elapsed event.
    public class ElapsedEventArgs : EventArgs
    {
        public ElapsedEventArgs(DateTime signalTime)
        {
            this.SignalTime = signalTime;
        }

        // Summary:
        //     Gets the time the System.Timers.Timer.Elapsed event was raised.
        //
        // Returns:
        //     The time the System.Timers.Timer.Elapsed event was raised.
        public DateTime SignalTime { get; private set; }
    }
}
