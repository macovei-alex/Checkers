﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.ViewModels
{
	internal class RulesVM : BaseViewModel
	{
		private static readonly string RulesFilePath = Path.GetFullPath(Properties.Settings.Default.RulesFilePath);

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