package residuum.org.premier.download;

import android.os.AsyncTask;
import android.support.annotation.Nullable;
import android.widget.Adapter;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by thomas on 05.02.16.
 */
public class Downloader extends AsyncTask<Void, Void, Table> {

    private final TableReceiver receiver;

    public Downloader(TableReceiver receiver){

        this.receiver = receiver;
    }

    private final String jsonUrl = "https://ix.residuum.org/pd/premierleague.php";

    @Override
    protected Table doInBackground(Void... params) {
        return download();
    }

    @Nullable
    private Table download() {
        String json = getJson(jsonUrl);
        if (json == null){
            return null;
        }
        try {
            JSONObject wrapper = new JSONObject(json);
            int maxPoints = wrapper.getInt("maxpts");
            JSONArray array = wrapper.getJSONArray("teams");
            List<Team> teams = new ArrayList<>();
            for (int i = 0; i < array.length(); ++i) {
                JSONObject team = array.getJSONObject(i);
                int position = team.getInt("pos");
                String name = team.getString("team");
                int points = team.getInt("pts");
                int goalsFor = team.getInt("gf");
                int goalsAgainst = team.getInt("ga");
                int goalDifference = team.getInt("gd");
                String filename = team.getString("filename");
                teams.add(new Team(position,name,goalsFor, goalsAgainst,goalDifference,points,filename));
            }
            return new Table(maxPoints, teams);
        } catch (JSONException e) {
            e.printStackTrace();
            return null;
        }
    }

    @Nullable
    private String getJson(String downloadUrl) {
        StringBuilder response = new StringBuilder();
        try {
            //Prepare the URL and the connection
            URL u = new URL(downloadUrl);
            HttpURLConnection conn = (HttpURLConnection) u.openConnection();

            if (conn.getResponseCode() == HttpURLConnection.HTTP_OK) {
                //Get the Stream reader ready
                BufferedReader input = new BufferedReader(new InputStreamReader(conn.getInputStream()), 8192);

                //Loop through the return data and copy it over to the response object to be processed
                String line = null;

                while ((line = input.readLine()) != null) {
                    response.append(line);
                }

                input.close();
            }
            return response.toString();
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    @Override
    protected void onPostExecute(Table table) {
        receiver.downloadComplete(table);
    }
}
