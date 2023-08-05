namespace task3;

public class Rules
{
    private readonly IList<string> _moves;

    public Rules(IList<string> moves)
    {
        _moves = moves;
    }

    public GameOutcome DetermineWinner(int userMove, int computerMove)
    {
        int separator = _moves.Count / 2;
        int upperBound = _moves.Count - 1;
        if (userMove == computerMove)
            return GameOutcome.Draw;
        // iterate half of the next moves and if one of them is a computer move, computer wins
        for (int i = userMove, j = 0; j <= separator; j++, i++)
        {
            if (i > upperBound)
                i = 0;
            if (i == computerMove)
                return GameOutcome.ComputerWinner;
        }
        // if we get here, user's move beats computer's move
        return GameOutcome.UserWinner;
    }

    public static string GetResultString(GameOutcome outcome)
    {
        var result = "";
        if (outcome == GameOutcome.UserWinner)
            result = "You win!";
        else if (outcome == GameOutcome.ComputerWinner)
            result = "You lose.";
        else
            result = "It's a draw!";
        return result;
    }

    public static string GetExplanation(string userMove, string computerMove, GameOutcome outcome)
    {
        string explanation = "";
        if (outcome == GameOutcome.UserWinner)
            explanation = $"{userMove} beats {computerMove}";
        else if (outcome == GameOutcome.ComputerWinner)
            explanation = $"{computerMove} beats {userMove}";
        else
            explanation = $"{computerMove} and {userMove} are the same";
        return explanation;
    }

}
