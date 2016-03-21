using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace PremierLeagueTable.NAudio
{
    class NAudioWasapi : IDisposable, INaudioOut
    {
        readonly WasapiOut _soundOutput;
        readonly IWaveProvider _waveProvider;

        public NAudioWasapi(IWaveProvider provider)
        {
            _waveProvider = provider;
            _soundOutput = new WasapiOut(AudioClientShareMode.Shared, 100);
        }

        ~NAudioWasapi()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool isDisposing)
        {
            if (_soundOutput != null)
            {
                _soundOutput.Stop();
                _soundOutput.Dispose();
            }
        }
        public void PrepareAudio()
        {
            if (_soundOutput.PlaybackState != PlaybackState.Stopped)
            {
                return;
            }
            _soundOutput.Init(_waveProvider);
            _soundOutput.Play();
        }
    }
}
