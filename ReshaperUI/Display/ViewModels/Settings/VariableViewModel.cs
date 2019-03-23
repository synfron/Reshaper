using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Utils;
using ReshaperCore.Providers;
using ReshaperCore.Vars;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class VariableViewModel : FormViewModel, IDisposable
	{
		private ICommand _saveCommand;
		private ICommand _deleteCommand;
		private string _variableName;
		private string _variableText;
		private Variables _globalVariables;
		private IVariable<string> _variable;
		private bool? _persistent;

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(Save, CanSave);
				}
				return _saveCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(Delete, CanDelete);
				}
				return _deleteCommand;
			}
		}

		protected override string PartialDisplayName
		{
			get
			{
				return (IsNew) ? "New..." : VariableName;
			}
		}

		private IVariable<string> Variable
		{
			get
			{
				return _variable;
			}
			set
			{
				if (value != null && value != _variable)
				{
					_variable = value;
					_variable.PropertyChanged += VariableChanged;
				}
			}
		}

		[Required(ErrorMessage = "'Variable Name' is required.")]
		public string VariableName
		{
			get
			{
				return _variableName;
			}
			set
			{
				this._variableName = value;
				this.OnPropertyChanged(nameof(VariableName));
			}
		}

		public string VariableText
		{
			get
			{
				return Flyweight.Get<string>(_variableText, Variable?.Value);
			}
			set
			{
				this._variableText = value;
				this.OnPropertyChanged(nameof(VariableText));
			}
		}

		public bool Persistent
		{
			get
			{
				return Flyweight.Get<bool>(_persistent, Variable?.Persistent);
			}
			set
			{
				this._persistent = value;
				this.OnPropertyChanged(nameof(Persistent));
			}
		}

		public VariableViewModel(string name = null)
		{
			SelfProvider selfProvider = new SelfProvider();

			_globalVariables = selfProvider.GetInstance().Variables;
			if (!string.IsNullOrEmpty(name))
			{
				Variable = _globalVariables.GetOrDefault<string>(name);
				IsNew = Variable == null;
				_variableName = name;
			}
			else
			{
				IsNew = true;
			}
		}

		public bool CanDelete()
		{
			return !IsNew;
		}

		private void Save()
		{
			IVariable<string> variable = Variable ?? _globalVariables.Add<string>(VariableName);
			variable.Value = VariableText;
			variable.Persistent = Persistent;
			if (IsNew)
			{
				Dispose();
			}
			else
			{
				IsSaved = true;
			}
		}

		private void Delete()
		{
			_globalVariables.Remove(VariableName);
		}

		private void VariableChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_variable.Value))
			{
				OnPropertyChanged(nameof(VariableText));
			}
		}

		public void Dispose()
		{
			if (_variable != null)
			{
				_variable.PropertyChanged -= VariableChanged;
				_variable = null;
			}
			VariableName = null;
			VariableText = null;
		}
	}
}
