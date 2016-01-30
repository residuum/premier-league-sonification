using System.Net.Http;
using Newtonsoft.Json;

namespace PremierLeagueTable.WebData
{
    public class Downloader
    {
        public delegate void DownloadReady(object sender, DownloadEventArgs args);

        public event DownloadReady Done;

        public async void Download()
        {
            using (HttpClient httpClient = new HttpClient())
            using (HttpResponseMessage response = await httpClient.GetAsync("http://ix.residuum.org/pd/premierleague.php"))
            using (HttpContent content = response.Content)
            {
                string jsonData = await content.ReadAsStringAsync();
                Table table = JsonConvert.DeserializeObject<Table>(jsonData);
                if (Done != null)
                {
                    Done(this, new DownloadEventArgs(table));
                }
            }
        }
    }
}
