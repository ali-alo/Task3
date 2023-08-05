using System.Text;
using ConsoleTables;

namespace task3;

public class Table
{
    private readonly IList<string> _moves;

    public Table(IList<string> moves)
    {
        _moves = moves;
    }

    public void DisplayPossibleMoves()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available moves:");
        for (int i = 0; i < _moves.Count; i++)
            sb.AppendLine($"{i + 1} - {_moves[i]}");
        sb.AppendLine("0 - exit");
        sb.AppendLine("? - help");
        Console.WriteLine(sb.ToString());
    }

    public void DisplayOutcomes()
    {
        Console.WriteLine(); // one empy line for better visualization
        ShowExample();
        Console.WriteLine("Below is a table of possible outcomes in this game");
        ConsoleTable table = CreateTable();
        table.Configure(opt => opt.EnableCount = false );
        table.Write();
    }

    private void ShowExample()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("To better understand the game, take a look at the example:");
        (int userRandomValue, int computerRandomValue, GameOutcome outcome) example = GetExampleValues();
        sb.AppendLine(GetExampleResultString(example.userRandomValue, example.computerRandomValue, example.outcome));
        Console.WriteLine(sb.ToString());
    }

    private string GetExampleResultString(int userRandomValue, int computerRandomValue, GameOutcome outcome)
    {
        string result = Rules.GetResultString(outcome);
        string userMove = _moves[userRandomValue];
        string computerMove = _moves[computerRandomValue];
        string explanation = Rules.GetExplanation(userMove, computerMove, outcome);
        return $"If you select {userMove} and computer selects {computerMove}, then you will see a messsage: {result}\n" + 
            $"This is because {explanation}.";
    }

    private (int, int, GameOutcome) GetExampleValues()
    {
        int userRandomValue = GetRandomNumber();
        int computerRandomValue = GetRandomNumber();
        var rules = new Rules(_moves);
        return (userRandomValue, computerRandomValue, 
            rules.DetermineWinner(userRandomValue, computerRandomValue));
    }

    private int GetRandomNumber() 
    {
        Random random = new Random();
        return random.Next(0, _moves.Count);
    }

    private ConsoleTable CreateTable()
    {
        List<string> headerRow = new List<string> {"You VS Computer"};
        headerRow.AddRange(_moves);
        var table = new ConsoleTable(headerRow.ToArray());
        AddRows(table);
        return table;
    }

    private void AddRows(ConsoleTable table)
    {
        Rules rules = new Rules(_moves);
        for (int i = 0; i < _moves.Count; i++)
        {
            List<string> row = new List<string> { _moves[i] };
            for (int j = 0; j < _moves.Count; j++)
                row.Add(GetResult(i, j, rules));
            table.AddRow(row.ToArray());
        }
    }

    private string GetResult(int userMove, int computerMove, Rules rules)
    {
        GameOutcome outcome = rules.DetermineWinner(userMove, computerMove);
            if (outcome == GameOutcome.UserWinner)
                return "Win";
            else if (outcome == GameOutcome.ComputerWinner)
                return "Lose";
            else
                return "Draw";
    }
}