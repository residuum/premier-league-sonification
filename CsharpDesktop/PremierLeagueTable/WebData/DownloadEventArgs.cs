using System;

namespace PremierLeagueTable.WebData
{
    public class DownloadEventArgs : EventArgs
    {
        public Table Table { get; private set; }

        public DownloadEventArgs(Table table)
        {
            Table = table;
        }
    }
}