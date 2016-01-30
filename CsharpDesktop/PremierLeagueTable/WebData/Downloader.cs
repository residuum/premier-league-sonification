using System.Net.Http;
using Newtonsoft.Json;

namespace PremierLeagueTable.WebData
{
    public class Downloader
    {
        public delegate void DownloadReady(object sender, DownloadEventArgs args);

        public event DownloadReady Ready;

        public async void Download()
        {
            using (HttpClient httpClient = new HttpClient())
            using (HttpResponseMessage response = await httpClient.GetAsync("http://ix.residuum.org/pd/premierleague.php"))
            using (HttpContent content = response.Content)
            {
                string jsonData = await content.ReadAsStringAsync();
                Table table = JsonConvert.DeserializeObject<Table>(jsonData);
                if (Ready != null)
                {
                    Ready(this, new DownloadEventArgs(table));
                }
            }
        }
    }
}
