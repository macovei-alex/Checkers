using System.IO;

namespace Checkers.ViewModels
{
	internal class RulesVM : BaseViewModel
	{
		private static readonly string _rulesFilePath = Path.GetFullPath(Properties.Settings.Default.RulesFilePath);
		public static string RulesFilePath => _rulesFilePath;

		private string _rulesText;
		public string RulesText
		{
			get => _rulesText;
			set
			{
				_rulesText = value;
				OnPropertyChanged(nameof(RulesText));
			}
		}

		public RulesVM()
		{
			RulesText = File.ReadAllText(RulesFilePath);
		}
	}
}
