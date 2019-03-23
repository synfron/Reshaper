using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ReshaperUI.Attributes;

namespace ReshaperUI.Converters
{
	public class AttributeToVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Visible;
			if (values[0] != null)
			{
				IDependentAttribute[] dependentAttributes = Attribute.GetCustomAttributes(values[0].GetType().GetProperty(parameter.ToString())).OfType<IDependentAttribute>().ToArray();
				if (dependentAttributes.Length > 0)
				{
					visibility = Visibility.Collapsed;
				}
				foreach (IDependentAttribute attribute in dependentAttributes)
				{
					if (attribute.IsActive(values[0]))
					{
						visibility = Visibility.Visible;
						break;
					}
				}
			}
			return visibility;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
