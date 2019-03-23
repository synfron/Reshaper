using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReshaperUI.Attributes;

namespace ReshaperUI.Display.ViewModels.Base
{
	public abstract class SourceModelViewModel : FormViewModel
	{
		public virtual void TranslateValues()
		{
			IEnumerable<PropertyInfo> translatableProperties = this.GetType().GetProperties().Where(
				property => Attribute.IsDefined(property, typeof(SourceModelPropertyAttribute)));
			PropertyInfo sourceModelProperty = this.GetType().GetProperties().FirstOrDefault(
				property => Attribute.IsDefined(property, typeof(SourceModelAttribute)));

			Object sourceModel = sourceModelProperty.GetValue(this);


			foreach (PropertyInfo translatableProperty in translatableProperties)
			{
				SourceModelPropertyAttribute attr = translatableProperty.GetCustomAttribute<SourceModelPropertyAttribute>();

				object value = translatableProperty.GetValue(this);

				attr.SetValue(sourceModel, value);
			}
			IsSaved = true;
		}
	}
}
