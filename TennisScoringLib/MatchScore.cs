namespace TennisScoringLib;

public class MatchScore
{
    private Dictionary<Player, TeamScore> _scores = new Dictionary<Player, TeamScore>();

    public Player CurrentServer { get; private set; }

    public MatchScore(Player p1, Player p2)
    {
        _scores.Add(p1, new TeamScore(p1));
        _scores.Add(p2, new TeamScore(p2));

        CurrentServer = p1;
    }

    private MatchScore(Player p1, TeamScore t1, Player p2, TeamScore t2, Player currentServer)
    {
        _scores.Add(p1, t1);
        _scores.Add(p2, t2);

        CurrentServer = currentServer;
    }

    public string GetGameScore()
    {
        var serverScore = _scores[CurrentServer];
        var otherScore = GetOppositePlayer(CurrentServer).Value;
        string score = $"{serverScore.GetRealGamePoints()}-{otherScore.GetRealGamePoints()}";

        if(score.Equals(ScoreConstants.FortyForty))
        {
            score = ScoreConstants.Deuce;
        }

        return score;
    }

    public string GetSetScore()
    {
        var serverScore = _scores[CurrentServer];
        var otherScore = GetOppositePlayer(CurrentServer).Value;
        string score = $"{serverScore.Sets}-{otherScore.Sets}";

        return score;
    }

    public bool IsWinning(Player player)
    {
        TeamScore winningScore = GetLargerScore(_scores[player], GetOppositePlayer(player).Value);
        return winningScore.Player.Equals(player);
    }

    private TeamScore GetLargerScore(TeamScore firstScore, TeamScore secondScore)
    {
        if (firstScore.Sets > secondScore.Sets)
        {
            return firstScore;
        }
        else if (firstScore.Sets < secondScore.Sets)
        {
            return secondScore;
        }
        // Else sets are equal, so continue checking

        if (firstScore.Games > secondScore.Games)
        {
            return firstScore;
        }
        else if (secondScore.Games > firstScore.Games)
        {
            return secondScore;
        }
        //Else games are equal, continue checking

        if (firstScore.GamePoints > secondScore.GamePoints)
        {
            return firstScore;
        }
        else if (secondScore.GamePoints > firstScore.GamePoints)
        {
            return secondScore;
        }

        return new TeamScore(Player.None);
    }

    public MatchScore AddPoint(Player scoringPlayer)
    {
        var score = _scores[scoringPlayer];

        if(score.GamePoints + 1 > ScoreConstants.MaxGameScore)
        {
            return PlayerWinsGame(scoringPlayer, score);
        }
        else
        {
            // Basic case where an additional point is added to the current game and that is all
            TeamScore newScore = score with { GamePoints = score.GamePoints + 1 };
            return With(scoringPlayer, newScore);
        }
    }

    private MatchScore PlayerWinsGame(Player scoringPlayer, TeamScore score)
    {
        // The game has been won and the set ticks up
        TeamScore scoringPlayerScore = score with { GamePoints = 0, Sets = score.Sets + 1 };
        TeamScore otherScore = GetOppositePlayer(scoringPlayer).Value with { GamePoints = 0 };

        var nextServer = GetOppositePlayer(CurrentServer).Key;
        return With(scoringPlayer, scoringPlayerScore, 
            GetOppositePlayer(scoringPlayer).Key, otherScore,
            nextServer);
    }

    private MatchScore With(Player p, TeamScore teamScore)
    {
        var otherPlayer = GetOppositePlayer(p);
        return new MatchScore(p, teamScore, otherPlayer.Key, otherPlayer.Value, CurrentServer);
    }

    private MatchScore With(Player scoringPlayer, TeamScore scoringPlayerScore, 
        Player otherPlayer, TeamScore otherScore, 
        Player server)
    {
        
        return new MatchScore(scoringPlayer, scoringPlayerScore, otherPlayer, otherScore, server);
    }

    private KeyValuePair<Player, TeamScore> GetOppositePlayer(Player p)
    {
        return _scores.Where(k => k.Key != p).First();
    }

}