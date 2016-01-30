using System.Collections.Generic;
using Newtonsoft.Json;

namespace PremierLeagueTable.WebData
{
    public class Table
    {
        [JsonProperty(PropertyName = "teams")]
        public IEnumerable<Team> Teams { get; set; }

        [JsonProperty(PropertyName = "maxpts")]
        public int Maxpoints { get; set; }
    }
}
