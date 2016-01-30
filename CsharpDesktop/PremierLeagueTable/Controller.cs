using System;
using PremierLeagueTable.PdBinding;
using PremierLeagueTable.WebData;

namespace PremierLeagueTable
{
    public class Controller : IDisposable
    {
        private Player _player;
        private NAudioBinding _naudio;

        public Controller(string assetsFolder)
        {
            Team.BaseFolder = assetsFolder;
            _player = new Player(assetsFolder);
            _player.TeamDisplaying += ((sender, args) =>
            {
                if (TeamDisplaying != null)
                {
                    TeamDisplaying(this, args);
                }

            });
            _player.TeamDisplayed += ((sender, args) =>
            {
                if (TeamDisplayed != null)
                {
                    TeamDisplayed(this, args);
                }
            });
            _naudio = new NAudioBinding(_player.BufferSize);

        }

        ~Controller()
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
            if (_player != null)
            {
                _player.Dispose();
            }
            if (_naudio != null)
            {
                _naudio.Dispose();
            }
        }

        private void DownloaderDone(object sender, DownloadEventArgs args)
        {
            if (Downloaded != null)
            {
                Downloaded(this, args);
            }
        }

        public delegate void DownloadReady(object sender, DownloadEventArgs args);

        public event DownloadReady Downloaded;

        public void Sonify(Table table)
        {
            _player.Start(table);
            _player.SetOutput(_naudio.Buffer);
        }

        public void Download()
        {
            _naudio.PrepareAudio();
            _player.BufferReady += ((sender, eventArgs) =>
            {
                _naudio.AddSamples(eventArgs.Output);
            });
            DownloadTable();
        }

        private void DownloadTable()
        {
            Downloader dl = new Downloader();
            dl.Done += DownloaderDone;
            dl.Download();
        }

        public delegate void TeamEvent(object sender, TeamEventArgs args);
        public event TeamEvent TeamDisplaying;
        public event TeamEvent TeamDisplayed;
    }
}
