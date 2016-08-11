package residuum.org.premier.puredata;

import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;
import android.support.annotation.NonNull;
import android.util.Log;

import org.puredata.android.io.AudioParameters;
import org.puredata.android.service.PdService;
import org.puredata.android.utils.PdUiDispatcher;
import org.puredata.core.PdBase;
import org.puredata.core.PdListener;
import org.puredata.core.utils.IoUtils;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.LinkedList;

import residuum.org.premier.R;
import residuum.org.premier.download.Table;
import residuum.org.premier.download.Team;

/**
 * Created by thomas on 08.02.16.
 */
public class Sonification {
    private final PdReceiver receiver;
    private final File dir;
    private ArrayList<Team> teams;
    private LinkedList<Team> teamStack;
    private int maxPoints;
    private Context activity;
    private final Object lock = new Object();
    private final Intent intent;
    private final PdUiDispatcher dispatcher = new PdUiDispatcher();
    private int patchHandle;

    public Sonification(PdReceiver receiver, Context activity) {
        this.receiver = receiver;
        this.activity = activity;
        dir = activity.getFilesDir();
        intent = new Intent(activity, PdService.class);
        activity.bindService(intent, pdConnection,Context.BIND_AUTO_CREATE);
        activity.startService(intent);
    }

    public void setTable(Table table) {
        maxPoints = table.getMaxPoints();
        teams = new ArrayList<>();
        for (Team team : table.getTeams()) {
            teams.add(team);
        }
    }

    public void play() {
        if (!pdService.isRunning()) {

        }
        pdService.startAudio();
        teamStack = new LinkedList<>();
        for (Team team : teams) {
            teamStack.add(team);
        }
        PdBase.sendFloat("max_points", maxPoints);
        sendNextTeam();
    }

    private void sendNextTeam() {
        Team current = teamStack.poll();
        if (current != null) {
            PdBase.sendList("data", current.getPdArguments());
            teamDisplaying(current);
        } else {
            sonificationDone();
        }
    }

    private void teamDisplaying(Team current) {
        receiver.teamDisplaying(current);
    }

    private void sonificationDone() {
        receiver.sonificationDone();
    }

    private PdService pdService;

    @NonNull
    private ServiceConnection pdConnection = new ServiceConnection() {
        @Override
        public void onServiceConnected(ComponentName name, IBinder service) {
            synchronized (lock) {
                pdService = ((PdService.PdBinder) service).getService();
                try {
                    initPd();
                    loadPatch();
                } catch (IOException e) {
                    Log.e("Pd", e.toString());
                }
            }
        }

        @Override
        public void onServiceDisconnected(ComponentName name) {
            // this method will never be called?!
        }
    };

    private void loadPatch() throws IOException {
        IoUtils.extractZipResource(activity.getResources().openRawResource(R.raw.pdlogic), dir, true);
        File patch = new File(dir, "sonification.pd");
        patchHandle = PdBase.openPatch(patch.getAbsolutePath());
    }

    private void initPd() throws IOException {
        // Configure the audio glue
        int sampleRate = AudioParameters.suggestSampleRate();

        AudioParameters.init(this.activity);
        pdService.initAudio(sampleRate, 0, 2, 10.0f);

        // Create and install the dispatcher
        PdBase.setReceiver(dispatcher);
        dispatcher.addListener("done", new PdListener.Adapter() {
            @Override
            public void receiveBang(String source) {
                if (source.equals("done")) {
                    sendNextTeam();
                }
            }
        });
    }

    public void clearPd() {
        PdBase.unsubscribe("done");
        PdBase.closeAudio();
        PdBase.closePatch(patchHandle);
        activity.unbindService(pdConnection);
    }
}
