using System;

class Program
{
	#region Constants
	const int WindowW = 50;
	const int WindowH = 22;
	const int TimePlay = 30;
	const string Board = @"
 ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗
 ║ 7 ║       ║ ║ 8 ║       ║ ║ 9 ║       ║
 ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║
     ║       ║     ║       ║     ║       ║
     ║       ║     ║       ║     ║       ║
     ╚═══════╝     ╚═══════╝     ╚═══════╝
 ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗
 ║ 4 ║       ║ ║ 5 ║       ║ ║ 6 ║       ║
 ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║
     ║       ║     ║       ║     ║       ║
     ║       ║     ║       ║     ║       ║
     ╚═══════╝     ╚═══════╝     ╚═══════╝
 ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗
 ║ 1 ║       ║ ║ 2 ║       ║ ║ 3 ║       ║
 ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║
     ║       ║     ║       ║     ║       ║
     ║       ║     ║       ║     ║       ║
     ╚═══════╝     ╚═══════╝     ╚═══════╝
";
	const string JavaNoob = @"
 ╔══─┐ 
 │o-o│ 
┌└───┘┐
││ J ││
";
	const string Empty = @"
       
       
       
       
";

	const string SelectLevel = @"
 ╔════════════════════════════════════════╗
 ║          SELECT LEVEL DIFFICULTY       ║
 ╚════════════════════════════════════════╝
        ╔═══════╗            ╔═══════╗
        ║   0   ║            ║   1   ║
        ╚═══════╝            ╚═══════╝
";
	#endregion

	static void Main(string[] args)
	{
		SetupConsole();
		RunGameLoop();
	}

	static void SetupConsole()
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		if (OperatingSystem.IsWindows())
		{
			Console.WindowWidth = Math.Max(Console.WindowWidth, WindowW);
			Console.WindowHeight = Math.Max(Console.WindowHeight, WindowH);
		}
	}

	static void RunGameLoop()
	{
		bool continuePlaying = true;
		while (continuePlaying)
		{
			int difficultyLevel = GetDifficultyLevel();
			continuePlaying = Play(difficultyLevel);
		}
	}

	static ConsoleKey ReadKeyFrom(ConsoleKey[] validKeys, string invalidInputText, int timeSleep)
	{
		ConsoleKey key;
		do
		{
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			key = keyInfo.Key;
			if (!validKeys.Contains(key))
			{
				Console.WriteLine(invalidInputText);
				System.Threading.Thread.Sleep(timeSleep);
				Console.SetCursorPosition(0, Console.CursorTop - 1);
				Console.Write(new string(' ', invalidInputText.Length));
				Console.SetCursorPosition(0, Console.CursorTop - 1);
			}
		}
		while (!validKeys.Contains(key));

		return key;
	}



	static int GetDifficultyLevel()
	{
		Console.WriteLine(SelectLevel + "\nEnter the difficulty level (0-1): ");
		ConsoleKey[] validKeys = { ConsoleKey.D0, ConsoleKey.D1 };
		string InvalidInputText = "Invalid input.  Please enter 0 for easy or 1 for hard.";
		int TimeSleep = 2000;
		ConsoleKey selectedKey = ReadKeyFrom(validKeys, InvalidInputText, TimeSleep);

		return selectedKey == ConsoleKey.D0 ? 0 : 1;
	}


	static bool Play(int difficultyLevel)
	{
		FillScreen("Whack A Mole (Java Noob Edition)\n\n" + Board, false);
		DateTime end = DateTime.Now.AddSeconds(TimePlay);
		int score = 0;
		int misses = 0;
		int moleLocation = Random.Shared.Next(1, 10);

		ConsoleKey[] validSelectionKeys = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9 };

		while (DateTime.Now < end)
		{
			int left = CalculateLeft(moleLocation);
			int top = CalculateTop(moleLocation);
			DisplayMole(JavaNoob, left, top);
			DisplayScoreAndMisses(score, misses);

			string InvalidInputText = "";
			int TimeSleep = 0;
			ConsoleKey selectedKey = ReadKeyFrom(validSelectionKeys, InvalidInputText, TimeSleep);
			int selection = selectedKey - ConsoleKey.D0;

			DisplayMole(Empty, left, top);
			if (moleLocation == selection)
			{
				score++;
			}
			else
			{
				if (difficultyLevel == 1)
				{
					score--;
				}
				misses++;
			}
			moleLocation = Random.Shared.Next(1, 10);
		}

		FillScreen($"Game Over. Score: {score}. Misses: {misses}\n\nPress [Enter] To Continue...", true);
		Console.ReadLine();
		return AskToPlayAgain();
	}

	static void DisplayMole(string moleGraphic, int x, int y)
	{
		string[] lines = moleGraphic.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		foreach (var line in lines)
		{
			Console.SetCursorPosition(x, y++);
			Console.Write(line);
		}
	}

	static void DisplayScoreAndMisses(int score, int misses)
	{
		int scorePositionLeft = 2;
		int scorePositionTop = WindowH;
		Console.SetCursorPosition(scorePositionLeft, scorePositionTop);
		Console.Write($"Score: {score}  Misses: {misses}");
	}

	static void FillScreen(string text, bool cursorVisible)
	{
		Console.CursorVisible = cursorVisible;
		Console.Clear();
		Console.WriteLine(text);
	}

	static int CalculateLeft(int moleLocation)
	{
		return 6 + ((moleLocation - 1) % 3) * 14;
	}

	static int CalculateTop(int moleLocation)
	{
		return 18 - (15 - ((9 - moleLocation) / 3) * 6);
	}

	static bool AskToPlayAgain()
	{
		Console.WriteLine("\nDo you want to play again? (Y/N)");
		ConsoleKey[] validKeys = { ConsoleKey.Y, ConsoleKey.N };
		string InvalidInputText = "Invalid Input";
		int TimeSleep = 2000;
		ConsoleKey selectedKey = ReadKeyFrom(validKeys, InvalidInputText, TimeSleep);

		return selectedKey == ConsoleKey.Y;
	}

}

