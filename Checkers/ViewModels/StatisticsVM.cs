using Checkers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public double WhiteWinsPercentage
		{
			get
			{
				if (Statistics.WhiteWins + Statistics.BlackWins == 0)
				{
					return 0;
				}
				return Statistics.WhiteWins / (double)(Statistics.WhiteWins + Statistics.BlackWins);
			}
		}

		public double BlackWinsPercentage
		{
			get
			{
				if (Statistics.WhiteWins + Statistics.BlackWins == 0)
				{
					return 0;
				}
				return Statistics.BlackWins / (double)(Statistics.WhiteWins + Statistics.BlackWins);
			}
		}

		public StatisticsVM()
		{
			Statistics = Statistics.FromDefaultFilePath();
		}
	}
}
