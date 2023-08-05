namespace task3;

public class Computer
{
    public int MakeMove(string[] moves) 
    {
        var random = new Random();
        int moveIndex = random.Next(0, moves.Length);
        string move = moves[moveIndex];
        return  moveIndex;
    }
}
