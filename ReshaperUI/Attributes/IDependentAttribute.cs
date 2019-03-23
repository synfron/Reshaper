using System.Collections.Generic;

namespace ReshaperUI.Attributes
{
	public interface IDependentAttribute
	{
		Dictionary<string, object> DependentPropertyValuePairs
		{
			get;
		}

		string DependentProperty
		{
			get;
		}

		object DependentValue
		{
			get;
		}

		string JsonDependentPropertyValuePairs
		{
			get;
		}
	}

	public static class IDependentAttributeExtensions
	{
		private static bool AreEqualValues(object currentValue, object expectedValue)
		{
			if (!(currentValue?.GetType().IsPrimitive ?? true))
			{
				currentValue = currentValue.ToString();
			}
			if (!(expectedValue?.GetType().IsPrimitive ?? true))
			{
				expectedValue = expectedValue.ToString();
			}
			return currentValue?.Equals(expectedValue) ?? currentValue == expectedValue;
		}

		public static IEnumerable<string> GetDepedentProperties(this IDependentAttribute attribute)
		{
			IEnumerable<string> dependentProperties = null;
			if (string.IsNullOrEmpty(attribute.JsonDependentPropertyValuePairs))
			{
				dependentProperties = new[] { attribute.DependentProperty };
			}
			else
			{
				dependentProperties = attribute.DependentPropertyValuePairs.Keys;
			}
			return dependentProperties;
		}

		public static bool IsActive(this IDependentAttribute attribute, object contextObject)
		{
			bool dependentValueMatches = true;
			if (!string.IsNullOrEmpty(attribute.DependentProperty))
			{

				object currentDependentValue = contextObject.GetType().GetProperty(attribute.DependentProperty).GetValue(contextObject);
				dependentValueMatches = AreEqualValues(currentDependentValue, attribute.DependentValue);
			}
			else if (!string.IsNullOrEmpty(attribute.JsonDependentPropertyValuePairs))
			{
				foreach (KeyValuePair<string, object> pair in attribute.DependentPropertyValuePairs)
				{
					object currentDependentValue = contextObject.GetType().GetProperty(pair.Key).GetValue(contextObject);
					dependentValueMatches &= AreEqualValues(currentDependentValue, pair.Value);
					if (!dependentValueMatches)
					{
						break;
					}
				}
			}
			else
			{
				dependentValueMatches = true;
			}
			return dependentValueMatches;
		}
	}
}
