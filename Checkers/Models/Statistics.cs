using Checkers.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
	public class Statistics
	{
		private static readonly string _statisticsFilePath = Path.GetFullPath(Properties.Settings.Default.StatisticsFilePath);
		public static string StatisticsFilePath => _statisticsFilePath;

		public int WhiteWins { get; }
		public int BlackWins { get; }
		public int MaxWinnerPiecesLeft { get; }

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

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static Statistics FromDefaultFilePath()
		{
			return FromJson(File.ReadAllText(StatisticsFilePath));
		}

		public static Statistics FromJson(string json)
		{
			return JsonConvert.DeserializeObject<Statistics>(json);
		}
	}
}
