namespace TennisScoringLib;

public class MatchScore
{
    private Dictionary<Player, TeamScore> _scores = new Dictionary<Player, TeamScore>();
    private Player matchWinner = Player.None;

    public Player CurrentServer { get; private set; }

    public bool MatchIsOver
    {
        get
        {
            return matchWinner != Player.None;
        }
    }

    public MatchScore(Player p1, Player p2)
    {
        _scores.Add(p1, new TeamScore(p1));
        _scores.Add(p2, new TeamScore(p2));

        CurrentServer = p1;
    }

    private MatchScore(Player p1, TeamScore t1, Player p2, TeamScore t2, Player currentServer, Player winner)
    {
        _scores.Add(p1, t1);
        _scores.Add(p2, t2);

        CurrentServer = currentServer;
        matchWinner = winner;
    }

    public string GetGameScore()
    {
        var serverScore = _scores[CurrentServer];
        var receiverScore = GetOppositePlayer(CurrentServer).Value;
        string score = $"{serverScore.GetRealGamePoints()}-{receiverScore.GetRealGamePoints()}";

        if(score.Equals(ScoreConstants.FortyForty))
        {
            score = ScoreConstants.Deuce;
        }

        return score;
    }

    public string GetSetScore()
    {
        var serverScore = _scores[CurrentServer];
        var receiverScore = GetOppositePlayer(CurrentServer).Value;
        string score = $"{serverScore.Games}-{receiverScore.Games}";

        return score;
    }

    public string GetMatchScore()
    {
        var serverScore = _scores[CurrentServer];
        var receiverScore = GetOppositePlayer(CurrentServer).Value;
        string score = $"{serverScore.Sets}-{receiverScore.Sets}";

        return score;
    }

    public bool IsWinning(Player player)
    {
        TeamScore winningScore = TeamScore.GetLargerScore(_scores[player], GetOppositePlayer(player).Value);
        return winningScore.Player.Equals(player);
    }

    public Player GetMatchWinner()
    {
        return matchWinner;
    }

    public MatchScore AddPoint(Player scoringPlayer)
    {
        if(MatchIsOver)
        {
            throw new MatchOverException();
        }

        TeamScore score = _scores[scoringPlayer];
        if (GameHasBeenWonByPointEnding(score))
        {
            return GetNewScoreAfterGameEnds(scoringPlayer, score);
        }
        else
        {
            return PointIsScoredInCurrentGame(scoringPlayer, score);
        }
    }

    private MatchScore GetNewScoreAfterGameEnds(Player scoringPlayer, TeamScore score)
    {
        (TeamScore scoringPlayerScore, TeamScore otherScore) = PlayerWinsGame(scoringPlayer, score);

        if (SetHasBeenWonByGameEnding(scoringPlayerScore))
        {
            (scoringPlayerScore, otherScore) = PlayerWinsSet(scoringPlayer, score, otherScore);
        }

        Player nextServer = GetNextServer();
        // When a set ends, it's possible the match may have ended
        // Best out of 5 sets
        if(scoringPlayerScore.Sets > 2)
        {
            matchWinner = scoringPlayer;
            nextServer = matchWinner; // The winner must have their score reported first
        }

        return With(scoringPlayer, scoringPlayerScore,
            GetNonScoringPlayer(scoringPlayer), otherScore,
            nextServer, matchWinner);
    }

    private MatchScore PointIsScoredInCurrentGame(Player scoringPlayer, TeamScore score)
    {
        TeamScore newScore = score with { GamePoints = score.GamePoints + 1 };
        return With(scoringPlayer, newScore);
    }

    private static bool SetHasBeenWonByGameEnding(TeamScore scoringPlayerScore)
    {
        return scoringPlayerScore.Games > ScoreConstants.MaxGamesInSet;
    }

    private static bool GameHasBeenWonByPointEnding(TeamScore score)
    {
        return score.GamePoints + 1 > ScoreConstants.MaxGameScore;
    }

    private Player GetNextServer()
    {
        return GetOppositePlayer(CurrentServer).Key;
    }

        private Player GetNonScoringPlayer(Player scoringPlayer)
    {
        return GetOppositePlayer(scoringPlayer).Key;
    }

    private (TeamScore scoringPlayerScore, TeamScore otherScore) PlayerWinsGame(Player scoringPlayer, TeamScore score)
    {
        // The game has been won and the set ticks up
        TeamScore scoringPlayerScore = score with 
        {   GamePoints = 0, 
            Games = score.Games + 1 };
        TeamScore otherScore = GetOppositePlayer(scoringPlayer).Value with { GamePoints = 0 };

        return (scoringPlayerScore, otherScore);
    }
    
    private (TeamScore scoringPlayerScore, TeamScore otherScore) PlayerWinsSet(Player scoringPlayer, TeamScore score, TeamScore otherScore)
    {
        TeamScore scoringPlayerScore = score with 
        {   GamePoints = 0, 
            Games = 0, 
            Sets = score.Sets + 1 };
        TeamScore newOtherScore = otherScore with 
        {   GamePoints = 0, 
            Games = 0 };

        return (scoringPlayerScore, newOtherScore);
    }

    private MatchScore With(Player p, TeamScore teamScore)
    {
        var otherPlayer = GetOppositePlayer(p);
        return new MatchScore(p, teamScore, otherPlayer.Key, otherPlayer.Value, CurrentServer, matchWinner);
    }

    private MatchScore With(Player scoringPlayer, TeamScore scoringPlayerScore, 
        Player otherPlayer, TeamScore otherScore, 
        Player server, 
        Player winner)
    {
        
        return new MatchScore(scoringPlayer, scoringPlayerScore, otherPlayer, otherScore, server, winner);
    }

    private KeyValuePair<Player, TeamScore> GetOppositePlayer(Player p)
    {
        return _scores.Where(k => k.Key != p).First();
    }

}