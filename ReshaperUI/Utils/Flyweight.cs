using System.Windows.Data;

namespace ReshaperUI.Utils
{
	public class Flyweight
	{
		public static T Get<T>(object currentValue, object defaultValue)
		{
			T value = (defaultValue != null) ? (T)defaultValue : default(T);
			if (currentValue is T && currentValue != null)
			{
				value = (T)currentValue;
			}
			return value;
		}

		public static T Get<T>(object currentValue, object defaultValue, IValueConverter converter)
		{
			T value = (defaultValue != null) ? (T)converter.Convert(defaultValue, typeof(T), null, null) : default(T);
			if (currentValue is T && currentValue != null)
			{
				value = (T)currentValue;
			}
			return value;
		}
	}
}
