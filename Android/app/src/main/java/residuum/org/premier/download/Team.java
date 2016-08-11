package residuum.org.premier.download;

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
        return new Object[]{filename + ".wav", position, goalsFor, goalsAgainst, goalDifference, points};
    }

    public String getFilename() {
        return filename + ".wav";
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
