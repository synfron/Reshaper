using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ReshaperUI.Converters
{
	public class StringToEscapedStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			return (value != null && value is string) ? Regex.Escape((string)value) : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			return (value != null && value is string) ? Regex.Unescape((string)value) : value;
		}
	}
}
