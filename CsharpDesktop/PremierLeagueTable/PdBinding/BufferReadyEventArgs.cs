using System;

namespace PremierLeagueTable.PdBinding
{
    public class BufferReadyEventArgs : EventArgs
    {
        public float[] Output { get; private set; }
        public BufferReadyEventArgs(float[] output)
        {
            Output = output;
        }
    }
}