using System;
using System.Collections.Generic;
using System.Linq;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	public abstract class ThenSetViewModel<T> : ThenViewModel<T> where T : ThenSet
	{
		private static IEnumerable<string> _settableMessageValueTypes;
		private bool? _useMessageValue;
		private bool? _promptValue;
		private string _promptDescription;
		private string _sourceMessageValue;
		private string _sourceMessageValueType;
		private string _sourceIdentifier;
		private bool? _useReplace;
		private string _regexPattern;
		private string _text;
		private string _replacementText;
		private int? _maxPromptWaitTime;

		public IEnumerable<string> SettableMessageValueTypes
		{
			get
			{
				if (_settableMessageValueTypes == null)
				{
					_settableMessageValueTypes = (Enum.GetValues(typeof(MessageValueType)) as MessageValueType[]).Select(valueTypes => (string)(new EnumToStringConverter().Convert(valueTypes, typeof(MessageValueType))));
				}
				return _settableMessageValueTypes;
			}
		}

		[SourceModelProperty("UseMessageValue")]
		public bool UseMessageValue
		{
			get
			{
				return Flyweight.Get<bool>(_useMessageValue, Then.UseMessageValue);
			}
			set
			{
				this._useMessageValue = value;
				this.OnPropertyChanged(nameof(UseMessageValue));
			}
		}

		[SourceModelProperty("PromptValue")]
		public bool PromptValue
		{
			get
			{
				return Flyweight.Get<bool>(_promptValue, Then.PromptValue);
			}
			set
			{
				this._promptValue = value;
				this.OnPropertyChanged(nameof(PromptValue));
			}
		}

		[SourceModelProperty("PromptDescription", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Prompt Description' is required.", DependentProperty = "PromptValue", DependentValue = true)]
		public string PromptDescription
		{
			get
			{
				return Flyweight.Get<string>(_promptDescription, Then.PromptDescription, new VariableStringToStringConverter());
			}
			set
			{
				this._promptDescription = value;
				this.OnPropertyChanged(nameof(PromptDescription));
			}
		}

		[SourceModelProperty("MaxPromptWaitTime")]
		[RequiredDependent(ErrorMessage = "'Max Time to Wait For Prompt Response' is required.", DependentProperty = "PromptValue", DependentValue = true)]
		public int MaxPromptWaitTime
		{
			get
			{
				return Flyweight.Get<int>(_maxPromptWaitTime, Then.MaxPromptWaitTime);
			}
			set
			{
				this._maxPromptWaitTime = value;
				this.OnPropertyChanged(nameof(MaxPromptWaitTime));
			}
		}

		[SourceModelProperty("SourceMessageValue", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Source Message Value' is required.", DependentProperty = "UseMessageValue", DependentValue = true)]
		public string SourceMessageValue
		{
			get
			{
				return Flyweight.Get<string>(_sourceMessageValue, Then.SourceMessageValue, new EnumToStringConverter());
			}
			set
			{
				this._sourceMessageValue = value;
				this.OnPropertyChanged(nameof(SourceMessageValue));
			}
		}

		[SourceModelProperty("SourceMessageValueType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Source Message Value Type' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"HTTP Body\",\"UseMessageValue\":true}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Type' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"Message\",\"UseMessageValue\":true}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Type' is required.", DependentProperty = "UseMessageValue", DependentValue = false)]
		public string SourceMessageValueType
		{
			get
			{
				return Flyweight.Get<string>(_sourceMessageValueType, Then.SourceMessageValueType, new EnumToStringConverter());
			}
			set
			{
				this._sourceMessageValueType = value;
				this.OnPropertyChanged(nameof(SourceMessageValueType));
			}
		}

		[SourceModelProperty("SourceIdentifier", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"HTTP Body\",\"UseMessageValue\":true, \"SourceMessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"HTTP Body\",\"UseMessageValue\":true, \"SourceMessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"Message\",\"UseMessageValue\":true, \"SourceMessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"Message\",\"UseMessageValue\":true, \"SourceMessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValue\":\"HTTP Header\",\"UseMessageValue\":true}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValueType\":\"JSON\",\"UseMessageValue\":false}")]
		[RequiredDependent(ErrorMessage = "'Source Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"SourceMessageValueType\":\"XML\",\"UseMessageValue\":false}")]
		public string SourceIdentifier
		{
			get
			{
				return Flyweight.Get<string>(_sourceIdentifier, Then.SourceIdentifier, new VariableStringToStringConverter());
			}
			set
			{
				this._sourceIdentifier = value;
				this.OnPropertyChanged(nameof(SourceIdentifier));
			}
		}

		[SourceModelProperty("UseReplace")]
		public bool UseReplace
		{
			get
			{
				return Flyweight.Get<bool>(_useReplace, Then.UseReplace);
			}
			set
			{
				this._useReplace = value;
				this.OnPropertyChanged(nameof(UseReplace));
			}
		}

		[SourceModelProperty("RegexPattern", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Regex Pattern' is required.", DependentProperty = "UseReplace", DependentValue = true)]
		public string RegexPattern
		{
			get
			{
				return Flyweight.Get<string>(_regexPattern, Then.RegexPattern, new VariableStringToStringConverter());
			}
			set
			{
				this._regexPattern = value;
				this.OnPropertyChanged(nameof(RegexPattern));
			}
		}

		[SourceModelProperty("Text", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string Text
		{
			get
			{
				return Flyweight.Get<string>(_text, Then.Text, new VariableStringToStringConverter());
			}
			set
			{
				this._text = value;
				this.OnPropertyChanged(nameof(Text));
			}
		}

		[SourceModelProperty("ReplacementText", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string ReplacementText
		{
			get
			{
				return Flyweight.Get<string>(_replacementText, Then.ReplacementText, new VariableStringToStringConverter());
			}
			set
			{
				this._replacementText = value;
				this.OnPropertyChanged(nameof(ReplacementText));
			}
		}
	}
}
