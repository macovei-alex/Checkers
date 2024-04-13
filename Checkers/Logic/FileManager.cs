using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Checkers.Utilities;

namespace Checkers.Logic
{
	internal class FileManager
	{
		public static string DefaultSavesFolderPath => Path.GetFullPath(Properties.Settings.Default.SavesFolderPath);

		private string SavesFolderPath { get; set; }

		public FileManager(string savesFolderPath)
		{
			SavesFolderPath = savesFolderPath;
		}

		public string SaveGame(string fileName, Game game, bool askForConfirmation = true)
		{
			string json = game.ToJson();
			string filePath = Path.Combine(SavesFolderPath, fileName);
			if (File.Exists(filePath) && askForConfirmation)
			{
				var result = MessageBox.Show("File already exists. Do you want to overwrite it?", "Question", MessageBoxButton.YesNo);

				if (result == MessageBoxResult.No)
				{
					Functions.Log($"Save action cancelled for file ( {filePath} )");
					return null;
				}
			}
			File.WriteAllText(filePath, json);

			Functions.Log($"Save action successful for file ( {filePath} )");
			return json;
		}

		public Game LoadGame(string fileName)
		{
			string filePath = Path.Combine(SavesFolderPath, fileName);
			if (!File.Exists(filePath))
			{
				Functions.Log($"Load action failed for file ( {filePath} ): File not found");
				throw new FileNotFoundException("File not found", filePath);
			}

			string json = File.ReadAllText(filePath);
			Game game = Game.FromJson(json);

			Functions.Log($"Load action successful for file ( {filePath} )");
			return game;
		}

		public void DeleteGame(string fileName)
		{
			string filePath = Path.Combine(SavesFolderPath, fileName);
			if (!File.Exists(filePath))
			{
				Functions.Log($"Delete action failed for file ( {filePath} ): File not found");
				throw new ArgumentException($"File {filePath} not found");
			}
			File.Delete(filePath);
		}
	}
}
