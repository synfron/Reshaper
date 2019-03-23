using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace ReshaperUI.Converters
{
	public class EnumToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			string descriptionValue = string.Empty;

			if (value != null)
			{
				FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
				if (fieldInfo != null)
				{
					descriptionValue = (fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[]).FirstOrDefault()?.Description;

					if (string.IsNullOrEmpty(descriptionValue))
					{
						descriptionValue = value.ToString();
					}
				}
			}

			return descriptionValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter = null, CultureInfo culture = null)
		{
			object enumValue = null;
			if (value != null)
			{
				FieldInfo fieldInfo = targetType.GetFields().FirstOrDefault(field => field.Name == value?.ToString() || field.GetCustomAttribute<DescriptionAttribute>()?.Description == value.ToString());
				if (fieldInfo != null)
				{
					if (!targetType.IsEnum)
					{
						targetType = targetType.GetGenericArguments().ElementAtOrDefault(0) ?? targetType;
					}
					enumValue = Enum.ToObject(targetType, (int)fieldInfo.GetValue(null));
				}
			}
			return enumValue;
		}
	}
}
