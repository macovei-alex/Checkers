using Checkers.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Checkers.Utilities.Enums;
using Checkers.Utilities;

namespace Checkers.Models
{
	internal class Statistics
	{
		private static readonly string _statisticsFilePath = Path.GetFullPath(Properties.Settings.Default.StatisticsFilePath);
		public static string StatisticsFilePath => _statisticsFilePath;

		public int WhiteWins { get; private set; }
		public int BlackWins { get; private set; }
		public int MaxWinnerPiecesLeft { get; private set; }

		public Statistics()
		{
			WhiteWins = 0;
			BlackWins = 0;
			MaxWinnerPiecesLeft = 0;
		}

		[JsonConstructor]
		public Statistics(int whiteWins, int blackWins)
		{
			WhiteWins = whiteWins;
			BlackWins = blackWins;
			MaxWinnerPiecesLeft = Math.Max(whiteWins, blackWins);
		}

		public static void UpdateStatistics(Game game)
		{
			int pieceCounter = 0;
			Colors winner = Functions.OppositeColor(game.Turn);
			foreach (Piece[] row in game.Board.Pieces)
			{
				foreach (Piece piece in row)
				{
					if (piece.Color == winner)
					{
						pieceCounter++;
					}
				}
			}

			UpdateStatistics(winner, pieceCounter);
		}

		public static void UpdateStatistics(Colors winner, int pieceCount)
		{
			Statistics statistics = FromDefaultFilePath();

			if (winner == Colors.White)
			{
				statistics.WhiteWins++;
			}
			else if (winner == Colors.Black)
			{
				statistics.BlackWins++;
			}

			if (pieceCount > statistics.MaxWinnerPiecesLeft)
			{
				statistics.MaxWinnerPiecesLeft = pieceCount;
			}

			statistics.SaveStatistics();
		}

		public static Statistics FromDefaultFilePath()
		{
			return FromJson(File.ReadAllText(StatisticsFilePath));
		}

		public static Statistics FromJson(string json)
		{
			return JsonConvert.DeserializeObject<Statistics>(json);
		}

		public void SaveStatistics()
		{
			File.WriteAllText(StatisticsFilePath, ToJson());
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
