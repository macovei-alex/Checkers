using Checkers.Logic;
using Checkers.Models;
using Checkers.Utilities;
using Checkers.ViewModels.Commands;
using Checkers.Views.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Checkers.ViewModels
{
	internal class FileManagerVM
	{
		private readonly GameVM _gameVM;
		public GameVM GameVM => _gameVM;

		private readonly FileManager _fileManager;
		public FileManager FileManager => _fileManager;

		public ICommand NewGameCommand { get; }
		public ICommand SaveGameCommand { get; }
		public ICommand LoadGameCommand { get; }
		public ICommand ShowStatisticsCommand { get; }
		public ICommand ShowAboutCommand { get; }
		public ICommand ShowRulesCommand { get; }

		public FileManagerVM(GameVM game)
		{
			_gameVM = game;
			_fileManager = new FileManager(FileManager.DefaultSavesFolderPath);

			NewGameCommand = new RelayCommand(NewGame);
			SaveGameCommand = new RelayCommand(SaveGame);
			LoadGameCommand = new RelayCommand(LoadGame);
			ShowStatisticsCommand = new RelayCommand(ShowStatistics);
			ShowAboutCommand = new RelayCommand(ShowAbout);
			ShowRulesCommand = new RelayCommand(ShowRules);
		}

		private void NewGame(object parameter)
		{
			var result = MessageBox.Show("Do you want to save the current game?", "Save current game", MessageBoxButton.YesNoCancel);
			if (result == MessageBoxResult.Yes)
			{
				SaveGame(null);
			}
			else if (result == MessageBoxResult.Cancel)
			{
				Functions.Log("New game action canceled");
				return;
			}

			GameVM.ReInitializeGame();
			Functions.Log("New game loaded");
		}

		private void SaveGame(object parameter)
		{
			FileDialog saveDialog = new SaveFileDialog
			{
				Filter = "Checkers save files (*.json)|*.json",
				InitialDirectory = FileManager.SavesFolderPath
			};

			if (saveDialog.ShowDialog() == false || saveDialog.FileName == null)
			{
				Functions.Log("Save action canceled");
				return;
			}

			_fileManager.SaveGame(saveDialog.FileName, GameVM.Game, false);
			Functions.Log("Saved game");
		}

		private void LoadGame(object parameter)
		{
			FileDialog openDialog = new OpenFileDialog
			{
				Filter = "Checkers save files (*.json)|*.json",
				InitialDirectory = FileManager.SavesFolderPath
			};

			if (openDialog.ShowDialog() == false || openDialog.FileName == null)
			{
				Functions.Log("Load action canceled");
				return;
			}

			GameVM.Game = _fileManager.LoadGame(openDialog.FileName, false);
			GameVM.ReInitializeGame(GameVM.Game);
			Functions.Log("Game loaded");
		}

		private void ShowStatistics(object parameter)
		{
			if (!File.Exists(Properties.Settings.Default.StatisticsFilePath))
			{
				MessageBox.Show($"The statistics file does not exist at {Statistics.StatisticsFilePath}");
				return;
			}

			new StatisticsWindow().ShowDialog();
			Functions.Log("Statistics loaded");
		}

		private void ShowAbout(object parameter)
		{
			new AboutWindow().ShowDialog();
			Functions.Log("Statistics loaded");
		}

		private void ShowRules(object parameter)
		{
			new RulesWindow().ShowDialog();
			Functions.Log("Rules loaded");
		}
	}
}
