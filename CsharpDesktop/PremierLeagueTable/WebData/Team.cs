using System.IO;
using Newtonsoft.Json;

namespace PremierLeagueTable.WebData
{
    public class Team
    {
        public static string BaseFolder { private get; set; }

        static string Subfolder { get { return "clubs"; } }

        string _fileName;
        [JsonProperty(PropertyName = "team")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string FileName
        {
            get { return string.Format("{0}.wav", _fileName); }
            set {
                _fileName = File.Exists(Path.Combine(BaseFolder, Subfolder, string.Format("{0}.wav", value))) ? value : "postponed";
            }
        }

        [JsonProperty(PropertyName = "pos")]
        public float Position { get; set; }
        [JsonProperty(PropertyName = "gf")]
        public float GoalsFor { get; set; }
        [JsonProperty(PropertyName = "ga")]
        public float GoalsAgainst { get; set; }
        [JsonProperty(PropertyName = "gd")]
        public float GoalDifference { get; set; }
        [JsonProperty(PropertyName = "pts")]
        public float Points { get; set; }

        public object[] ToPdArgs()
        {
            return new object[] {FileName, Position, GoalsFor, GoalsAgainst, GoalDifference, Points};
        }
    }
}