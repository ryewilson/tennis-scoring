using Microsoft.VisualStudio.TestTools.UnitTesting;
using TennisScoringLib;

namespace TennisScoringTest;

[TestClass]
public class TennisMatchTests
{
    private (TennisMatch, Player, Player) GetTestMatch()
    {
        var p1 = new Player("Johnboy");
        var p2 = new Player("AkechiKun");

        var match = new TennisMatch(p1, p2);
        match.ServesFirst(p1);

        return (match, p1, p2);
    }

    [TestMethod]
    public void CreateMatch_success()
    {
        var match = GetTestMatch();
        Assert.IsNotNull(match);
    }

    [TestMethod]
    public void GetScore_isZero()
    {
        (var match, _, _) = GetTestMatch();
        MatchScore score = match.GetScore();
    }

    [TestMethod]
    public void AddPlayer_success()
    {
        (var match, _, _) = GetTestMatch();
        Assert.AreEqual(2, match.Players);
    }

    [TestMethod]
    public void SetServer_isSet()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);
        Assert.AreEqual(match.FirstServer, p1);
    }

    [TestMethod]
    public void ScoreOnePoint()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);
        MatchScore score = match.ScorePoint(p1);
        Assert.IsTrue(score.IsWinning(p1));
        
        Assert.AreEqual("15-0", score.GetGameScore());
        
    }

     [TestMethod]
    public void ScoreTwoPoints()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);
        
        _ = match.ScorePoint(p1);
        MatchScore score = match.ScorePoint(p1);
        
        Assert.IsTrue(score.IsWinning(p1));
        Assert.AreEqual("30-0", score.GetGameScore());
    }

     [TestMethod]
    public void ScoreThreePoints()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        MatchScore score = match.ScorePoint(p1);
        
        Assert.IsTrue(score.IsWinning(p1));
        Assert.AreEqual("40-0", score.GetGameScore()); 
    }

     [TestMethod]
    public void ScoreThreePointsAndOnePoint_is40_15()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        var score = match.ScorePoint(p2);
        
        Assert.IsTrue(score.IsWinning(p1));
        Assert.AreEqual("40-15", score.GetGameScore()); 
    }

    [TestMethod]
    public void ScoreThreeAndThree_isDeuce()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);

        _ = match.ScorePoint(p2);
        _ = match.ScorePoint(p2);
        var score = match.ScorePoint(p2);
        
        Assert.IsFalse(score.IsWinning(p1));
        Assert.IsFalse(score.IsWinning(p2));
        Assert.AreEqual("Deuce", score.GetGameScore()); 
    }

    [TestMethod]
    public void ScoreOneAndThree_is15_40()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p2);
        _ = match.ScorePoint(p2);
        var score = match.ScorePoint(p2);
        
        Assert.IsTrue(score.IsWinning(p2));
        Assert.AreEqual("15-40", score.GetGameScore()); 
    }

    [TestMethod]
    public void ScoreFour_winsFirstGame()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        // Score the point to win the game
        MatchScore score = match.ScorePoint(p1);
        
        Assert.IsTrue(score.IsWinning(p1));
        // Because the game was won, game score goes back to 0-0
        Assert.AreEqual("0-0", score.GetGameScore()); 
        // Score is 0-1 because after the first game ends
        // the service switches
        Assert.AreEqual("0-1", score.GetSetScore());
    }   

    [TestMethod]
    public void ScoreFourAndOne_winsFirstGameAndStartsNext()
    {
        (var match, Player p1, Player p2) = GetTestMatch();
        match.ServesFirst(p1);

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        // Score the point to win the game
        _ = match.ScorePoint(p1);
        // Score first point in next game by OTHER player
        MatchScore score = match.ScorePoint(p2);
        
        Assert.IsTrue(score.IsWinning(p1));
        // Scores swap because server switched
        Assert.AreEqual("15-0", score.GetGameScore()); 
        Assert.AreEqual("0-1", score.GetSetScore());
    } 

     [TestMethod]
    public void TwoGamesComplete_TiedScore()
    {
        (var match, Player p1, Player p2) = GetTestMatch();

        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        _ = match.ScorePoint(p1);
        // Score the point to win the game
        _ = match.ScorePoint(p1);
        // Score first point in next game by OTHER player
        MatchScore score = match.ScorePoint(p2);
        
        Assert.IsTrue(score.IsWinning(p1));
        // Scores swap because server switched
        Assert.AreEqual("15-0", score.GetGameScore()); 
        Assert.AreEqual("0-1", score.GetSetScore());
    } 

    // Winning a set
    // Winning a match
    // Ad scoring
}