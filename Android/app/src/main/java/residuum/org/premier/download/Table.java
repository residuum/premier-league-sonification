package residuum.org.premier.download;

/**
 * Created by thomas on 05.02.16.
 */
public final class Table {
    private final int maxPoints;
    private final Iterable<Team> teams;

    public Table(int maxPoints, Iterable<Team> teams){
        this.maxPoints = maxPoints;
        this.teams = teams;
    }

    public Iterable<Team> getTeams() {
        return teams;
    }

    public int getMaxPoints() {
        return maxPoints;
    }
}
