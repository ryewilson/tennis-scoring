
namespace TennisScoringLib;

public record TeamScore
{
        public TeamScore(Player p)
        {
            Player = p;
        }
        public Player Player {get;init;}
        public uint GamePoints {get;init;}
        public uint Games {get; init;}
        public uint Sets {get; init;}

        public string GetRealGamePoints()
        {
            switch(GamePoints)
            {
                case 0:
                    return ScoreConstants.Zero.ToString();
                case 1:
                    return ScoreConstants.Fifteen.ToString();
                case 2:
                    return ScoreConstants.Thirty.ToString();
                case 3:
                    return ScoreConstants.Forty.ToString();
                default: // Anything greater than 3
                    throw new InvalidGamePointsException();
            }
        }

    public static TeamScore GetLargerScore(TeamScore firstScore, TeamScore secondScore)
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
}