using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Checkers.Models;
using static Checkers.Utilities.Enums;

namespace Checkers.Utilities
{
	internal static class Functions
	{
		public static string AssetsFolderPath => Properties.Settings.Default.AssetsFolderPath;

		public enum Pages
		{
			StartingPage,
			GamePage,
		}

		public static string GetPageName(Pages page)
		{
			switch (page)
			{
				case Pages.StartingPage:
					return "StartingPage";
				case Pages.GamePage:
					return "GamePage";
				default:
					return null;
			}
		}

		public static Colors OppositeColor(Colors color)
		{
			return color == Colors.White ? Colors.Black : Colors.White;
		}

		public static ImageTypes MakeImageType(Types type, Colors color)
		{
			return (ImageTypes)((int)type * 10 + (int)color);
		}

		public static BitmapImage LoadImage(string filePath)
		{
			try
			{
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.EndInit();
				return bitmap;
			}
			catch (Exception ex)
			{
				throw new ApplicationException($"Failed to load image at {filePath}", ex);
			}
		}

		public static bool IsBetween(int value, int min, int max)
		{
			return value >= min && value < max;
		}

		public static void Log(string message)
		{
			Console.WriteLine(message);
		}

		public static void AddRange<T>(ObservableCollection<T> collection, IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				collection.Add(item);
			}
		}

		public static void Clear<T>(ObservableCollection<T> collection)
		{
			for (int i = collection.Count - 1; i >= 0; i--)
			{
				collection.RemoveAt(i);
			}
		}
	}
}
