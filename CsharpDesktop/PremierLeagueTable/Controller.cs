using System;
using PremierLeagueTable.NAudio;
using PremierLeagueTable.PdBinding;
using PremierLeagueTable.WebData;

namespace PremierLeagueTable
{
    public class Controller : IDisposable
    {
        readonly Player _player;
        readonly INaudioOut _naudio;
        static Controller _instance;

        public static Controller GetInstance(string assetsFolder, bool useJack)
        {
            if (_instance == null)
            {
                _instance = new Controller(assetsFolder, useJack);
            }
            _instance.BaseFolder = assetsFolder;
            return _instance;
        }

        string BaseFolder
        {
            set
            {
                Team.BaseFolder = value;
            } 
        }

        Controller(string assetsFolder, bool useJack)
        {
            _player = new Player(assetsFolder);
            _player.TeamStarting += ((sender, args) =>
            {
                if (TeamStarting != null)
                {
                    TeamStarting(this, args);
                }

            });
            _player.TeamPlayed += ((sender, args) =>
            {
                if (TeamPlayed != null)
                {
                    TeamPlayed(this, args);
                }
            });
            _player.TableDone += ((sender, args) =>
            {
                if (TableDone != null)
                {
                    TableDone(this, args);
                }
            });
            if (useJack)
            {
                _naudio = new NAudioJack(new PdProvider(_player));
            }
            else
            {
                _naudio = new NAudioWasapi(new PdProvider(_player));

            }

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

        void DownloaderDone(object sender, DownloadEventArgs args)
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
            //_player.SetOutput(_naudio.Buffer);
        }

        public void Download()
        {
            _naudio.PrepareAudio();
            //_player.BufferReady += ((sender, eventArgs) =>
            //{
            //    _naudio.AddSamples(eventArgs.Output);
            //});
            DownloadTable();
        }

        void DownloadTable()
        {
            Downloader dl = new Downloader();
            dl.Done += DownloaderDone;
            dl.Download();
        }

        public delegate void TeamEvent(object sender, TeamEventArgs args);
        public event TeamEvent TeamStarting;
        public event TeamEvent TeamPlayed;
        public event TeamEvent TableDone;
    }
}
