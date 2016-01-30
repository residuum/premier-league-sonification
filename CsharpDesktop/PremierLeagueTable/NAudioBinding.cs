using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace PremierLeagueTable
{
    class NAudioBinding : IDisposable
    {
        public float[] Buffer { get; private set; }
        private readonly WasapiOut _soundOutput;
        private BufferedWaveProvider _audioBuffer;

        public NAudioBinding(int bufferSize)
        {

            Buffer = new float[bufferSize];
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
            _audioBuffer = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                BufferDuration = TimeSpan.FromSeconds(10),
                DiscardOnBufferOverflow = true
            };
            _soundOutput.Init(_audioBuffer);
            _soundOutput.Play();
        }

        public void AddSamples(float[] output)
        {
            _audioBuffer.AddSamples(PcmFromFloat(output), 0, output.Length * 4);
        }

        private static byte[] PcmFromFloat(float[] buffer)
        {
            WaveBuffer wavebuffer = new WaveBuffer(buffer.Length * 4);
            for (var i = 0; i < buffer.Length; i++)
            {
                wavebuffer.FloatBuffer[i] = buffer[i];
            }
            return wavebuffer.ByteBuffer;
        }
    }
}
