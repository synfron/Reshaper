using System;
using System.Linq;
using System.Reflection;
using ReshaperUI.Attributes;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels;
using ReshaperUI.Display.ViewModels.Base;

namespace ReshaperUI.Utils
{
	public class SourceModelSaveCommand<T> : RelayCommand<T>
	{

		public SourceModelSaveCommand(Action<T> execute = null, Func<bool> canExecute = null) : base(execute, canExecute)
		{

		}

		public override void Execute(object parameter)
		{
			SourceModelViewModel model = parameter as SourceModelViewModel;
			if (model != null)
			{
				model.TranslateValues();

				if (model.IsNew && _execute != null)
				{
					model.IsNew = false;

					PropertyInfo sourceModelProperty = model.GetType().GetProperties().FirstOrDefault(property => Attribute.IsDefined(property, typeof(SourceModelAttribute)));

					Object sourceModel = sourceModelProperty.GetValue(model);
					_execute((T)sourceModel);
				}
			}
		}
	}
}
