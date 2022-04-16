namespace TennisScoringLib;

public class Player
{
    public string Name {get;}
    public Player(string name)
    {
        Name = name;
    }

    public static Player None {get;} = new Player("None");
}