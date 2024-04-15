using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Checkers.Models;
using Checkers.ViewModels;
using static Checkers.Utilities.Enums;

namespace Checkers.Utilities
{
	internal static class Functions
	{
		public static string AssetsFolderPath => Properties.Settings.Default.AssetsFolderPath;

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

		public static void CollectionChanged<T>(NotifyCollectionChangedEventArgs e,
			Predicate<T> addPredicate,
			Predicate<T> removePredicate,
			params ObservableCollection<T>[] collectionsCheckExists)
		{
			T elem;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					elem = (T)e.NewItems[e.NewItems.Count - 1];
					addPredicate(elem);
					break;

				case NotifyCollectionChangedAction.Remove:
					elem = (T)e.OldItems[e.OldItems.Count - 1];
					bool found = collectionsCheckExists?.Any(collection => collection.Contains(elem)) ?? false;
					if (!found)
					{
						removePredicate(elem);
					}
					break;
			}
		}
	}
}
