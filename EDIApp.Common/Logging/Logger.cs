namespace EDIApp.Common.Logging
{
	/// <summary>
	/// App logger.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Log debug message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Debug(string message)
		{
			Console.Write($"[{DateTime.UtcNow}] ");
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.Write($" D ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine($" {message}");
		}

		/// <summary>
		/// Log warning message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Warning(string message)
		{
			Console.Write($"[{DateTime.UtcNow}] ");
			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.Write($" W ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine($" {message}");
		}

		/// <summary>
		/// Log error message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Error(string message)
		{
			Console.Write($"[{DateTime.UtcNow}] ");
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.Write($" E ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine($" {message}");
		}

		/// <summary>
		/// Log information message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Information(string message)
		{
			Console.Write($"[{DateTime.UtcNow}] ");
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.Write($" I ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine($" {message}");
		}

		public static void UserInput(out string input)
		{
			Console.Write($"[{DateTime.UtcNow}] Input data => ");
			
			input = Console.ReadLine() ?? "";
		}
	}
}
