using System;
using System.Globalization;
using System.Windows.Data;

namespace ReshaperUI.Converters
{
	class ListToObservableCollectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			object listObj = null;
			if (value != null)
			{
				listObj = Activator.CreateInstance(targetType, value);
			}
			return listObj;
		}

		public object ConvertBack(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			object listObj = null;
			if (value != null)
			{
				listObj = Activator.CreateInstance(targetType, value);
			}
			return listObj;
		}
	}
}
