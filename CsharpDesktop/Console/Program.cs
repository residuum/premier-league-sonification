using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using PremierLeagueTable.WebData;
using PremierLeagueTable.PdBinding;

namespace ConsoleImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Team.BaseFolder = AssetsFolder;
            _player = new Player(AssetsFolder);
            _buffer = new float[_player.BufferSize];
            using (_soundOutput = new WasapiOut(AudioClientShareMode.Shared, 100))
            {
                _audioBuffer = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
                {
                    BufferDuration = TimeSpan.FromSeconds(10),
                    DiscardOnBufferOverflow = true
                };
                _soundOutput.Init(_audioBuffer);
                _soundOutput.Play();
                _player.BufferReady += ((sender, eventArgs) =>
                {
                    float[] buffer = eventArgs.Output;
                    _audioBuffer.AddSamples(PcmFromFloat(buffer), 0, buffer.Length*4);
                });
                Downloader dl = new Downloader();
                dl.Ready += Downloader_Ready;
                dl.Download();
                Console.Read();
                _soundOutput.Stop();
            }
        }

        private static Player _player;
        private static float[] _buffer;
        private static WasapiOut _soundOutput;
        private static BufferedWaveProvider _audioBuffer;

        private static string AssetsFolder
        {
            get {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.GetFullPath(assemblyFolder + ConfigurationManager.AppSettings["baseFolder"]);
            }
        }

        private static void Downloader_Ready(object sender, DownloadEventArgs args)
        {
            _player.Start(args.Table);
            _player.SetOutput(_buffer);
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
