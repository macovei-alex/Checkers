﻿using Newtonsoft.Json;
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

		public string SavesFolderPath { get; private set; }

		public FileManager(string savesFolderPath)
		{
			SavesFolderPath = savesFolderPath;
		}

		public void SaveGame(string fileName, Game game, bool appendToDirectory)
		{
			string json = game.ToJson();
			string filePath = appendToDirectory ? Path.Combine(SavesFolderPath, fileName) : fileName;
			File.WriteAllText(filePath, json);

			Functions.Log($"Save action successful for file ( {filePath} )");
			return;
		}

		public Game LoadGame(string fileName, bool appendToDirectory)
		{
			string filePath = appendToDirectory ? Path.Combine(SavesFolderPath, fileName) : fileName;
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
