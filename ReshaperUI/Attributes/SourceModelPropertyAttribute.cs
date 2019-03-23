using System;

namespace ReshaperUI.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SourceModelPropertyAttribute : Attribute
	{

		public string PropertyName { get; private set; }
		public Type ConverterType { get; set; }
		public bool UseConvertBack { get; set; }

		public SourceModelPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		public void SetValue(Object sourceModel, object value)
		{

			Type sourceModelType = sourceModel.GetType();
			if (ConverterType != null)
			{
				object coverterObj = Activator.CreateInstance(ConverterType, null);
				string convertMethod = UseConvertBack ? "ConvertBack" : "Convert";
				value = ConverterType.GetMethod(convertMethod).Invoke(coverterObj, new object[] { value, sourceModelType.GetProperty(PropertyName).PropertyType, null, null });
			}

			sourceModelType.GetProperty(PropertyName)?.SetValue(sourceModel, value);
		}
	}
}
