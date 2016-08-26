using System;
using System.Linq;
using NAudio.Utils;
using NAudio.Wave;
using PremierLeagueTable.PdBinding;

namespace PremierLeagueTable.NAudio
{
    class PdProvider : IWaveProvider
    {
        readonly CircularBuffer _circularBuffer;
        readonly float[] _buffer;
        readonly int _minBuffer;
        readonly Player _player;

        public PdProvider(Player player)
        {
            _buffer = new float[player.BufferSize];
            _player = player;
            _player.BufferReady += PdBufferReady;
            _circularBuffer =  new CircularBuffer(_player.SampleRate * 5); // 5 seconds should be enough for anybody
            _minBuffer =  _player.SampleRate / 2; // 0.5 second
            RefillBuffer();
        }

        void RefillBuffer()
        {
            if (_circularBuffer.Count < _minBuffer)
            {
                _player.Process(_buffer);
            }
        }

        void PdBufferReady(object sender, BufferReadyEventArgs args)
        {
            float[] newSamples = args.Output;
            _circularBuffer.Write(PcmFromFloat(newSamples), 0, newSamples.Length*4);
            RefillBuffer();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var read = _circularBuffer.Read(buffer, offset, count);
            RefillBuffer();
            return read;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return WaveFormat.CreateIeeeFloatWaveFormat(_player.SampleRate, 2);
            }
        }

        static byte[] PcmFromFloat(float[] pdOutput)
        {
            WaveBuffer wavebuffer = new WaveBuffer(pdOutput.Length * 4);
            for (var i = 0; i < pdOutput.Length; i++)
            {
                wavebuffer.FloatBuffer[i] = pdOutput[i];
            }
            return wavebuffer.ByteBuffer;
        }
    }
}
