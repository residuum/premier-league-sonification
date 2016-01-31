using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace PremierLeagueTable.NAudio
{
    class NAudioBinding : IDisposable
    {
        readonly WasapiOut _soundOutput;
        readonly IWaveProvider _waveProvider;

        public NAudioBinding(IWaveProvider provider)
        {
            _waveProvider = provider;
            _soundOutput = new WasapiOut(AudioClientShareMode.Shared, 100);
        }

        ~NAudioBinding()
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
