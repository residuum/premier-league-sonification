using System;

namespace PremierLeagueTable.WebData
{
    public class TeamEventArgs : EventArgs
    {
        public Team Team { get; private set; }

        public TeamEventArgs(Team team)
        {
            Team = team;
        }
    }
}