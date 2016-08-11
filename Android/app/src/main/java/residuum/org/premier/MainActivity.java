package residuum.org.premier;

import android.graphics.Typeface;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import org.puredata.android.service.PdService;

import java.io.File;

import residuum.org.premier.download.Downloader;
import residuum.org.premier.download.Table;
import residuum.org.premier.download.TableReceiver;
import residuum.org.premier.download.Team;
import residuum.org.premier.puredata.PdReceiver;
import residuum.org.premier.puredata.Sonification;

public class MainActivity extends AppCompatActivity implements TableReceiver, PdReceiver {

    private Button loadButton;
    private Button sonifyButton;
    private Sonification sonification;
    private TableLayout tableDisplay;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        sonification = new Sonification(this, this);
        loadButton = (Button)findViewById(R.id.load_data);
        loadButton.setOnClickListener(loadButtonClicked);
        sonifyButton = (Button)findViewById(R.id.sonify);
        sonifyButton.setOnClickListener(sonifyButtonClicked);
        tableDisplay = (TableLayout)findViewById(R.id.display);
    }

    @Override
    public void onDestroy(){
    super.onDestroy();
        sonification.clearPd();
    }

    View.OnClickListener loadButtonClicked = new View.OnClickListener(){
        public void onClick(View view){
            sonifyButton.setVisibility(View.INVISIBLE);
            loadButton.setEnabled(false);
            new Downloader(MainActivity.this).execute();
        }
    };

    View.OnClickListener sonifyButtonClicked = new View.OnClickListener(){
        public void onClick(View view){
            loadButton.setEnabled(false);
            sonifyButton.setEnabled(false);

            prepareTable();
            sonification.play();
        }
    };

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_about) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void downloadComplete(Table table) {
        if (table != null) {
            sonifyButton.setVisibility(View.VISIBLE);
            prepareTable();

            sonification.setTable(table);
        }
        loadButton.setEnabled(true);
    }

    @Override
    public void sonificationDone() {
        loadButton.setEnabled(true);
        sonifyButton.setEnabled(true);
    }

    private void prepareTable() {
        tableDisplay.removeAllViews();
        TableRow row = new TableRow(this);
        row.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.FILL_PARENT));
        row.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.FILL_PARENT));
        TextView pos = getTextView("POS");
        pos.setTypeface(null, Typeface.BOLD);
        row.addView(pos);
        TextView teamName = getTextView("Team");
        teamName.setTextAlignment(View.TEXT_ALIGNMENT_TEXT_START);
        teamName.setTypeface(null, Typeface.BOLD);
        row.addView(teamName);
        TextView goalsFor = getTextView("GF");
        goalsFor.setTypeface(null, Typeface.BOLD);
        row.addView(goalsFor);
        TextView goalsAgainst = getTextView("GA");
        goalsAgainst.setTypeface(null, Typeface.BOLD);
        row.addView(goalsAgainst);
        TextView goalDifference = getTextView("GD");
        goalDifference.setTypeface(null, Typeface.BOLD);
        row.addView(goalDifference);
        TextView points = getTextView("PTS");
        points.setTypeface(null, Typeface.BOLD);
        row.addView(points);
        tableDisplay.addView(row, new TableLayout.LayoutParams(TableLayout.LayoutParams.FILL_PARENT, TableLayout.LayoutParams.WRAP_CONTENT));
    }

    @Override
    public void teamDisplaying(Team team) {
        if (team == null){
            return;
        }
        TableRow row = new TableRow(this);
        row.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.FILL_PARENT));
        TextView pos = getTextView(Integer.toString(team.getPosition()));
        row.addView(pos);
        TextView teamName = getTextView(team.getName());
        teamName.setTextAlignment(View.TEXT_ALIGNMENT_TEXT_START);
        row.addView(teamName);
        TextView goalsFor = getTextView(Integer.toString(team.getGoalsFor()));
        row.addView(goalsFor);
        TextView goalsAgainst = getTextView(Integer.toString(team.getGoalsAgainst()));
        row.addView(goalsAgainst);
        TextView goalDifference = getTextView(Integer.toString(team.getGoalDifference()));
        row.addView(goalDifference);
        TextView points = getTextView(Integer.toString(team.getPoints()));
        row.addView(points);
        tableDisplay.addView(row, new TableLayout.LayoutParams(TableLayout.LayoutParams.FILL_PARENT, TableLayout.LayoutParams.WRAP_CONTENT));
    }

    @NonNull
    private TextView getTextView(String text) {
        TextView textView = new TextView(this);
        textView.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT));
        textView.setPadding(8, 3, 8, 3);
        textView.setText(text);
        textView.setTextAlignment(View.TEXT_ALIGNMENT_TEXT_END);
        return textView;
    }
}
