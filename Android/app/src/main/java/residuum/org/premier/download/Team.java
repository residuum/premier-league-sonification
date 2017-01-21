package residuum.org.premier.download;

import java.io.File;

/**
 * Created by thomas on 05.02.16.
 */
public final class Team {
    private final int position;
    private final String name;
    private final int goalsFor;
    private final int goalsAgainst;
    private final int goalDifference;
    private final int points;
    private final String filename;

    private static String path;
    public static void setPath(String path){
        Team.path = path;
    }

    public Team(int position, String name, int goalsFor, int goalsAgainst, int goalDifference, int points, String filename ){

        this.position = position;
        this.name = name;
        this.goalsFor = goalsFor;
        this.goalsAgainst = goalsAgainst;
        this.goalDifference = goalDifference;
        this.points = points;
        this.filename = filename;
    }

    public Object[] getPdArguments() {
        String filename = "postponed.wav";
        File wavFile = new File(path + File.separator + this.filename + ".wav");
        if (wavFile.exists()){
            filename = this.filename + ".wav";
        }
        return new Object[]{
                filename, position, goalsFor, goalsAgainst, goalDifference, points
        };
    }

    public int getPoints() {
        return points;
    }

    public int getGoalDifference() {
        return goalDifference;
    }

    public int getGoalsAgainst() {
        return goalsAgainst;
    }

    public int getGoalsFor() {
        return goalsFor;
    }

    public String getName() {
        return name;
    }

    public int getPosition() {
        return position;
    }
}
