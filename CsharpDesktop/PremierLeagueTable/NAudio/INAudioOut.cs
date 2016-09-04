using System;

namespace PremierLeagueTable.NAudio
{
    interface INaudioOut : IDisposable
    {
        void PrepareAudio();
    }
}
