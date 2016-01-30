using System;
using System.Configuration;
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
            Downloader dl = new Downloader();
            dl.Ready += Downloader_Ready;
            dl.Download();
            Console.Read();
        }

        private static Player _player;

        private static string AssetsFolder
        {
            get { return ConfigurationManager.AppSettings["baseFolder"]; }
        }

        private static void Downloader_Ready(object sender, DownloadEventArgs args)
        {
            _player.Start(args.Table);
        }
    }
}
