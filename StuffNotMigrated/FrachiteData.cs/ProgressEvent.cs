using System;

namespace FrachiteData.cs
{
    public class ThresholdReachedEventArgs : EventArgs
    {
        public ThresholdReachedEventArgs( int p )
        {
            Progress = Math.Max(0, Math.Min(100, p));
        }

        public int Progress { get; set; }
    }
}