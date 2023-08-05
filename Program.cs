using task3;

internal class Program
{
    static Table table = default!;
    static string errorMessage = string.Empty;
    
    private static void Main(string[] args)
    { 
        if (!ValidateArguments(args))
        {
            ShowErrorMessage();
            return;
        }
        table = new Table(args);
        StartGame(args);
    }

    static void StartGame(string[] args)
    {
        Computer computer = new Computer();
        int computerMove = computer.MakeMove(args);
        HmacGenerator hmac = new HmacGenerator(args[computerMove]);
        Console.WriteLine($"HMAC: {hmac.Hmac}");
        ShowGameMenu(args, computerMove);
        Console.WriteLine($"HMAC KEY: {hmac.RandomKey}");
    }

    static void ShowGameMenu(string[] args, int computerMove)
    {
        table.DisplayPossibleMoves();
        string input = GetUserInput(args);
        int convertedInput = ConvertInput(input);
        ServeUserCommand(convertedInput, computerMove, args);
    }

    static int ConvertInput(string input)
    {
        if (input == "?")
            return -2;
        int result = Int32.Parse(input); // at this point we know the input is an int
        return result -1; // 0 was used for exit, need to subtract 1 to match array indices
    }

    static void ServeUserCommand(int userInput, int computerMove, string[] args)
    {
        if (userInput == -2) // question mark was passed
        {
            table.DisplayOutcomes();
            ShowGameMenu(args, computerMove);
        }
        else if (userInput == -1)
        {
            Exit();
        }
        else
        {
            Rules rules = new Rules(args);
            GameOutcome gameOutcome = rules.DetermineWinner(userInput, computerMove);
            ShowGameResult(args[userInput], args[computerMove], gameOutcome);
        }
    }

    static bool ValidateArguments(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0)
        {
            errorMessage = "To play a game you must pass at least three arguments and the total number of arguments cannot be even";
            return false;
        }
        foreach (var arg in args)
        {
            if (args.Distinct().Count() != args.Length)
            {
                errorMessage = "All arguments must be unique (case-insensitive)";
                return false;
            }
        }
        return true;
    }

    static string GetUserInput(string[] args)
    {
        Console.Write("Enter your move: ");
        var input = Console.ReadLine();
        while (!ValidateInput(input, args.Length))
        {
            ShowErrorMessage();
            table.DisplayPossibleMoves();
            Console.Write("Enter a valid move: ");
            input = Console.ReadLine();
        }
        return input!;
    }

    static bool ValidateInput(string? input, int argsLength)
    {
        if (input == null)
        {
            errorMessage = "You need to pass value to play";
            return false;
        }

        if (!int.TryParse(input, out int userMove) && input != "?")
        {
            errorMessage = "You can only pass a number or a question mark. See the list of possible commands below";
            return false;
        }

        if (userMove > argsLength || userMove < 0)
        {
            errorMessage = "The passed number is unavailable. See the list of possible commands below";
            return false;
        }
        return true;
    }

    static void ShowErrorMessage()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(errorMessage);
        Console.ResetColor();
    }

    static void ShowGameResult(string userMove, string computerMove, GameOutcome gameOutcome)
    {
        Console.WriteLine($"Your move: {userMove}");
        Console.WriteLine($"Computer move: {computerMove}");
        Console.WriteLine(Rules.GetResultString(gameOutcome));
    }

    static void Exit()
    {
        Console.WriteLine("Exiting the app...");
        Thread.Sleep(1000);
        Environment.Exit(0);
    }
}