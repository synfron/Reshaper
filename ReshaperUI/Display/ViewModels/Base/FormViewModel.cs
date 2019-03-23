using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ReshaperUI.Attributes;

namespace ReshaperUI.Display.ViewModels.Base
{
	public abstract class FormViewModel : ObservableViewModel, IDataErrorInfo
	{
		private readonly Dictionary<string, Tuple<Func<FormViewModel, object>, ValidationAttribute[]>> _validators = new Dictionary<string, Tuple<Func<FormViewModel, object>, ValidationAttribute[]>>();
		private bool? _isValid;
		private bool _isSaved = true;
		private ValidationContext _validateContext = null;
		private bool _isNew;

		public bool IsSaved
		{
			get
			{
				return _isSaved && !IsNew;
			}
			set
			{
				if (_isSaved != value)
				{
					_isSaved = value;
					OnPropertyChanged(nameof(DisplayName));
				}
			}
		}

		protected abstract string PartialDisplayName
		{
			get;
		}

		public string DisplayName
		{
			get
			{
				return PartialDisplayName + ((IsSaved || IsNew) ? string.Empty : " *");
			}
		}

		public bool IsValid
		{
			get
			{
				if (_isValid == null)
				{
					Validate();
				}
				return _isValid.Value;
			}
			protected set
			{
				_isValid = value;
				OnPropertyChanged(nameof(IsValid));
			}
		}

		public virtual bool IsNew
		{
			get
			{
				return _isNew;
			}
			set
			{
				_isNew = value;
				OnPropertyChanged(nameof(IsNew));
			}
		}

		public string Error
		{
			get
			{
				List<string> errors = new List<string>();
				foreach (Tuple<Func<FormViewModel, object>, ValidationAttribute[]> validationData in _validators.Values)
				{
					object value = validationData.Item1(this);

					errors.AddRange(validationData.Item2.Where(attr => attr.GetValidationResult(value, _validateContext) != ValidationResult.Success).Select(attr => attr.ErrorMessage));
				}
				return string.Join(Environment.NewLine, errors);
			}
		}

		public string this[string propertyName]
		{
			get
			{
				Tuple<Func<FormViewModel, object>, ValidationAttribute[]> validationData = null;
				string error = string.Empty;
				IEnumerable<string> errors = null;
				if (this._validators.TryGetValue(propertyName, out validationData))
				{
					object value = validationData.Item1(this);
					errors = validationData.Item2.Where(attr => attr.GetValidationResult(value, _validateContext) != ValidationResult.Success).Select(attr => attr.ErrorMessage);
					error = string.Join(Environment.NewLine, errors);
				}
				return error;
			}
		}

		public FormViewModel()
		{
			SetValidators();
			_validateContext = new ValidationContext(this);
		}

		private void SetValidators()
		{
			PropertyInfo[] properties = this.GetType().GetProperties();
			foreach (PropertyInfo property in properties)
			{
				ValidationAttribute[] validations = GetValidations(property);
				if (validations.Length > 0)
				{
					_validators[property.Name] = new Tuple<Func<FormViewModel, object>, ValidationAttribute[]>(GetValueGetter(property), validations);
				}
			}
		}

		private ValidationAttribute[] GetValidations(PropertyInfo property)
		{
			return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
		}

		private Func<FormViewModel, object> GetValueGetter(PropertyInfo property)
		{
			return new Func<FormViewModel, object>(model => property.GetValue(model, null));
		}

		public bool CanSave()
		{
			return IsValid;
		}

		private IEnumerable<string> GetDependentProperties(string propertyName)
		{
			return this.GetType().GetProperties().Where(
				property => property.GetCustomAttributes().Any(attr => (attr as IDependentAttribute)?.GetDepedentProperties().Contains(propertyName) ?? false)).Select(property => property.Name);
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			foreach (string dependentPropertyName in GetDependentProperties(propertyName))
			{
				base.OnPropertyChanged(dependentPropertyName);
			}

			if (propertyName != "IsValid")
			{
				Validate();
			}
			switch (propertyName)
			{
				case "IsValid":
				case "IsSaved":
				case "DisplayName":
				case "IsNew":
					break;
				default:
					IsSaved = false;
					break;
			}
		}

		public void Validate()
		{
			bool isValid = true;
			foreach (Tuple<Func<FormViewModel, object>, ValidationAttribute[]> validationData in _validators.Values)
			{
				object value = validationData.Item1(this);
				isValid &= validationData.Item2.All(validator => validator.GetValidationResult(value, _validateContext) == ValidationResult.Success);
			}
			IsValid = isValid;
		}

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
