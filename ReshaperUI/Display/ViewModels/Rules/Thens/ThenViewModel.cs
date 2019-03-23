using System;
using System.Windows.Input;
using ReshaperCore.Rules.Thens;
using ReshaperUI.Attributes;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	public abstract class ThenViewModel : RuleOperationViewModel
	{
	}

	public abstract class ThenViewModel<T> : ThenViewModel where T : Then
	{
		private SourceModelSaveCommand<Then> _saveCommand;

		[SourceModel]
		public T Then { get; private set; }

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<Then>(RuleModel.Rule.Thens.Add, CanSave);
				}
				return _saveCommand;
			}
		}

		public override object GetModel()
		{
			return Then;
		}

		private void SetModel(T then)
		{
			Then = then ?? Activator.CreateInstance<T>();
			if (then == null)
			{
				IsNew = true;
			}
		}

		public virtual void SetModels(RuleViewModel rule, T then = null)
		{
			SetRuleModel(rule);
			SetModel(then);
		}
	}
}
