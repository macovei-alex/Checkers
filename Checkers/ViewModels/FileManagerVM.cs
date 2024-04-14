using Checkers.Logic;
using Checkers.Utilities;
using Checkers.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public FileManagerVM(GameVM game)
		{
			_gameVM = game;
			_fileManager = new FileManager(FileManager.DefaultSavesFolderPath);

			NewGameCommand = new RelayCommand(NewGame);
			SaveGameCommand = new RelayCommand(SaveGame);
			LoadGameCommand = new RelayCommand(LoadGame);
		}

		private void NewGame(object parameter)
		{
			var result = MessageBox.Show("Do you want to save the current game?", "Save  current game", MessageBoxButton.YesNoCancel);
			if (result == MessageBoxResult.Yes)
			{
				SaveGame(null);
			}
			else if (result == MessageBoxResult.Cancel)
			{
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
				MessageBox.Show("Save action cancelled");
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
				MessageBox.Show("Load action cancelled");
				return;
			}

			GameVM.Game = _fileManager.LoadGame(openDialog.FileName, false);
			GameVM.ReInitializeGame(GameVM.Game);
			Functions.Log("Game loaded");
		}
	}
}
