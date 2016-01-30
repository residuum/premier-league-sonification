﻿using System;
using System.Collections.Generic;
using System.IO;
using PremierLeagueTable.WebData;

namespace PremierLeagueTable.PdBinding
{
    public class Player : IDisposable
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
                if (TeamDisplayed != null)
                {
                    TeamDisplayed(this, args);
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

        public void SetOutput(float[] output)
        {
            _operation.SetOutput(output, Ticks);
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
        public event TeamEvent TeamDisplaying;
        public event TeamEvent TeamDisplayed;

        void NextTeam()
        {
            lock (_teams)
            {
                if (_teams.Count > 0)
                {
                    Team team = _teams.Dequeue();
                    if (TeamDisplaying != null)
                    {
                        TeamDisplaying(this, new TeamEventArgs(team));
                    }
                    _operation.SetCurrentTeam(team);
                }
            }
        }
    }
}
