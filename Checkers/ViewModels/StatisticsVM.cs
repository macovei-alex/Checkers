using Checkers.Models;

namespace Checkers.ViewModels
{
	internal class StatisticsVM : BaseViewModel
	{
		private Statistics _statistics;
		public Statistics Statistics
		{
			get => _statistics;
			set
			{
				_statistics = value;
				OnPropertyChanged(nameof(Statistics));
			}
		}

		public float WhiteWinsPercentage
		{
			get
			{
				if (Statistics.WhiteWins + Statistics.BlackWins == 0)
				{
					return 0;
				}
				return 100 * Statistics.WhiteWins / (float)(Statistics.WhiteWins + Statistics.BlackWins);
			}
		}

		public float BlackWinsPercentage
		{
			get
			{
				if (Statistics.WhiteWins + Statistics.BlackWins == 0)
				{
					return 0;
				}
				return 100 * Statistics.BlackWins / (float)(Statistics.WhiteWins + Statistics.BlackWins);
			}
		}

		public StatisticsVM()
		{
			Statistics = Statistics.FromDefaultFilePath();
		}
	}
}
