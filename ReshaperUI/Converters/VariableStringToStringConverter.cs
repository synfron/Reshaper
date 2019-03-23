using System;
using System.Globalization;
using System.Windows.Data;
using ReshaperCore.Vars;

namespace ReshaperUI.Converters
{
	public class VariableStringToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			VariableString varStr = value as VariableString;
			string convertedValue = null;
			if (varStr != null)
			{
				convertedValue = varStr?.GetFormattedString();
			}
			return convertedValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			VariableString varStr = null;
			if (value != null)
			{
				varStr = VariableString.GetAsVariableString(value.ToString());
			}
			return varStr;
		}
	}
}
