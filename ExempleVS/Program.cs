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


	static int GetDifficultyLevel()
	{
		while (true)
		{
			FillScreen(SelectLevel + "\nEnter the difficulty level (0-1): ", true);
			if (int.TryParse(Console.ReadLine(), out int difficultyLevel) && (difficultyLevel == 0 || difficultyLevel == 1))
			{
				return difficultyLevel;
			}
			else
			{
				Console.WriteLine("Invalid input. Please enter 0 for easy or 1 for hard.");
				System.Threading.Thread.Sleep(2000); // To let the user read the message.
			}
		}
	}

	static bool Play(int difficultyLevel)
	{
		FillScreen("Whack A Mole (Java Noob Edition)\n\n" + Board, false);
		DateTime end = DateTime.Now.AddSeconds(TimePlay);
		int score = 0;
		int misses = 0;
		int moleLocation = Random.Shared.Next(1, 10);

		while (DateTime.Now < end)
		{
			int left = CalculateLeft(moleLocation);
			int top = CalculateTop(moleLocation);
			DisplayMole(JavaNoob, left, top);
			DisplayScoreAndMisses(score, misses);

			if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int selection) && selection >= 1 && selection <= 9)
			{
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
		Console.WriteLine("\nDo you want to play again? (yes/no)");
		string answer = Console.ReadLine().Trim().ToLower();
		return answer.StartsWith("y");
	}
}

