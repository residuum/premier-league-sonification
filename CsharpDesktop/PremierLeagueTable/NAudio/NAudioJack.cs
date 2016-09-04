using System;
using JackSharp;
using NAudio.Jack;
using NAudio.Wave;

namespace PremierLeagueTable.NAudio
{
    class NAudioJack : IDisposable, INaudioOut
    {
        readonly JackOut _soundOutput;
        readonly IWaveProvider _waveProvider;
        readonly Processor _jackProcessor;

        public NAudioJack(IWaveProvider provider)
        {
            _waveProvider = provider;
            _jackProcessor = new Processor("PremierLeague", audioOutPorts:2, autoconnect:true);
            _soundOutput = new JackOut(_jackProcessor);
        }

        ~NAudioJack()
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
            if (_jackProcessor != null)
            {
                _jackProcessor.Stop();
                _jackProcessor.Dispose();
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
