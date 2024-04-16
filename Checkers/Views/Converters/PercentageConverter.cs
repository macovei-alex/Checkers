using System;
using System.Windows.Data;

namespace Checkers.Views.Converters
{
	internal class PercentageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string formatted = ((float)value).ToString("0.00");
			return $"{formatted}%";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
