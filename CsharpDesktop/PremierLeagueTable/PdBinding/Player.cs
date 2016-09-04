using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PremierLeagueTable.WebData;

namespace PremierLeagueTable.PdBinding
{
    class Player : IDisposable
    {
        readonly PdOperation _operation;
        Queue<Team> _teams;

        public int BufferSize
        {
            get { return 2 * Ticks * BlockSize; }
        }

        public int Ticks
        {
            get { return 4; }
        }

        public int BlockSize
        {
            get { return _operation.BlockSize; }
        }

        public int SampleRate
        {
            get { return _operation.SampleRate; }
        }

        public Player(string assetsFolder)
        {
            _operation = new PdOperation(Path.Combine(assetsFolder, "sonification.pd"));
            _operation.BufferReady += ((sender, args) =>
            {
                if (BufferReady != null)
                {
                    BufferReady(this, args);
                }
            });
            _operation.GetNext += ((sender, args) =>
            {
                if (TeamPlayed != null)
                {
                    TeamPlayed(this, args);
                }
                NextTeam();
            });
        }

        public delegate void Buffered(object sender, BufferReadyEventArgs args);
        public event Buffered BufferReady;

        ~Player()
        {
            Dispose(false);
        }

        void SetPdOutput(object state)
        {
            float[] buffer = state as float[];
            if (buffer == null)
            {
                return;
            }

            _operation.Process(buffer, Ticks);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool isDisposing)
        {
            if (_operation != null)
            {
                _operation.Dispose();
            }
        }

        public void Start(Table table)
        {
            _operation.SetMaxPoints(table.Maxpoints);
            _teams = new Queue<Team>(table.Teams);
            NextTeam();
        }

        public delegate void TeamEvent(object sender, TeamEventArgs args);
        public event TeamEvent TeamStarting;
        public event TeamEvent TeamPlayed;
        public event TeamEvent TableDone;

        void NextTeam()
        {
            lock (_teams)
            {
                if (_teams.Count > 0)
                {
                    Team team = _teams.Dequeue();
                    if (TeamStarting != null)
                    {
                        TeamStarting(this, new TeamEventArgs(team));
                    }
                    _operation.SetCurrentTeam(team);
                }
                else if (TableDone != null)
                {
                    TableDone(this, new TeamEventArgs(null));
                }
            }
        }

        public void Process(float[] buffer)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(SetPdOutput), buffer);
        }
    }
}
