namespace TennisScoringLib;
public class TennisMatch
{

    private List<Player> _players = new List<Player>();
    private MatchScore _score;
    public Player FirstServer {get; private set;}

    public int Players 
    {
        get
        {
            return _players.Count;
        }
    }

    public TennisMatch(Player p1, Player p2)
    {
        _players.Add(p1);
        _players.Add(p2);
        FirstServer = p1;

        _score = new MatchScore(p1, p2);
    }

    public MatchScore GetScore()
    {
        return _score;
    }

    public MatchScore ScorePoint(Player p1)
    {
        if(_players.Contains(p1))
        {
            _score = _score.AddPoint(p1);
            return _score;
        }
        else
        {
            throw new InvalidOperationException("Cannot score point for Player that is not in the match");
        }
    }

    public void ServesFirst(Player p1)
    {
        FirstServer = p1;
    }
}
