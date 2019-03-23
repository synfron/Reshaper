using System;
using System.Windows.Input;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	public abstract class WhenViewModel : RuleOperationViewModel
	{
	}

	public abstract class WhenViewModel<T> : WhenViewModel where T : When
	{
		private bool? _negate;
		private bool? _useOrCondition;
		private SourceModelSaveCommand<When> _saveCommand;

		[SourceModel]
		public T When { get; private set; }

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<When>(RuleModel.Rule.Whens.Add, CanSave);
				}
				return _saveCommand;
			}
		}

		[SourceModelProperty("UseOrCondition")]
		public bool? UseOrCondition
		{
			get
			{
				return Flyweight.Get<bool>(_useOrCondition, When.UseOrCondition);
			}
			set
			{
				this._useOrCondition = value;
				this.OnPropertyChanged(nameof(UseOrCondition));
			}
		}

		[SourceModelProperty("Negate")]
		public bool? Negate
		{
			get
			{
				return Flyweight.Get<bool>(_negate, When.Negate);
			}
			set
			{
				this._negate = value;
				this.OnPropertyChanged(nameof(Negate));
			}
		}

		public override object GetModel()
		{
			return When;
		}

		private void SetModel(T when)
		{
			When = when ?? Activator.CreateInstance<T>();
			if (when == null)
			{
				IsNew = true;
			}
		}

		public virtual void SetModels(RuleViewModel rule, T when = null)
		{
			SetRuleModel(rule);
			SetModel(when);
		}
	}
}
