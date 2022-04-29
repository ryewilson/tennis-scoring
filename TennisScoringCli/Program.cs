// Add support for https://github.com/commandlineparser/commandline

using TennisScoringLib;

if (args.Length > 2)
{
    if(args[0] == "newmatch")
    {
        var p1 = new Player(args[1]);
        var p2 = new Player(args[2]);
        TennisMatch match = new TennisMatch(p1, p2);

        using(var file = new StreamWriter("match.txt"))
        {
            file.WriteLine(p1.Name + " vs. " + p2.Name);
            file.WriteLine(match.GetScore().GetGameScore());
        }
        return 1;
    }
    return 0;
}
else
{
    return 0;
}