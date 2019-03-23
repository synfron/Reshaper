using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Matches Text"), Export(typeof(WhenViewModel<WhenMatchesText>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenMatchesTextViewModel : WhenViewModel<WhenMatchesText>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private static IEnumerable<string> _settableMessageValueTypes;
		private string _messageValueType;
		private string _identifier;
		private bool? _useMessageValue;
		private string _messageValue;
		private string _matchType;
		private string _regexPattern;
		private string _sourceText;
		private string _matchText;

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

		public IEnumerable<string> MatchTypes
		{
			get
			{
				return (Enum.GetValues(typeof(MatchType)) as MatchType[]).Select(matchType => (string)(new EnumToStringConverter().Convert(matchType, typeof(MatchType))));
			}
		}

		[SourceModelProperty("MessageValueType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Message Value Type' is required.", DependentProperty = "MessageValue", DependentValue = "HTTP Body")]
		[RequiredDependent(ErrorMessage = "'Message Value Type' is required.", DependentProperty = "MessageValue", DependentValue = "Message")]
		[RequiredDependent(ErrorMessage = "'Message Value Type' is required.", DependentProperty = "UseMessageValue", DependentValue = false)]
		public string MessageValueType
		{
			get
			{
				return Flyweight.Get<string>(_messageValueType, When.MessageValueType, new EnumToStringConverter());
			}
			set
			{
				this._messageValueType = value;
				this.OnPropertyChanged(nameof(MessageValueType));
			}
		}

		[SourceModelProperty("Identifier", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"HTTP Header\",\"UseMessageValue\":true}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"HTTP Body\",\"MessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"HTTP Body\",\"MessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"Message\",\"MessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"Message\",\"MessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"UseMessageValue\":false,\"MessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"UseMessageValue\":false,\"MessageValueType\":\"XML\"}")]
		public string Identifier
		{
			get
			{
				return Flyweight.Get<string>(_identifier, When.Identifier, new VariableStringToStringConverter());
			}
			set
			{
				this._identifier = value;
				this.OnPropertyChanged(nameof(Identifier));
			}
		}

		[SourceModelProperty("UseMessageValue")]
		public bool UseMessageValue
		{
			get
			{
				return Flyweight.Get<bool>(_useMessageValue, When.UseMessageValue);
			}
			set
			{
				this._useMessageValue = value;
				this.OnPropertyChanged(nameof(UseMessageValue));
			}
		}

		[SourceModelProperty("MessageValue", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Message Value' is required.", DependentProperty = "UseMessageValue", DependentValue = true)]
		public string MessageValue
		{
			get
			{
				return Flyweight.Get<string>(_messageValue, When.MessageValue, new EnumToStringConverter());
			}
			set
			{
				this._messageValue = value;
				this.OnPropertyChanged(nameof(MessageValue));
			}
		}

		[SourceModelProperty("MatchType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string MatchType
		{
			get
			{
				return Flyweight.Get<string>(_matchType, When.MatchType, new EnumToStringConverter());
			}
			set
			{
				this._matchType = value;
				this.OnPropertyChanged(nameof(MatchType));
			}
		}

		[SourceModelProperty("RegexPattern", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Regex Pattern' is required.", DependentProperty = "MatchType", DependentValue = "Regex")]
		public string RegexPattern
		{
			get
			{
				return Flyweight.Get<string>(_regexPattern, When.RegexPattern, new VariableStringToStringConverter());
			}
			set
			{
				this._regexPattern = value;
				this.OnPropertyChanged(nameof(RegexPattern));
			}
		}

		[SourceModelProperty("SourceText", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[VisibleDependent(DependentProperty = "UseMessageValue", DependentValue = false)]
		public string SourceText
		{
			get
			{
				return Flyweight.Get<string>(_sourceText, When.SourceText, new VariableStringToStringConverter());
			}
			set
			{
				this._sourceText = value;
				this.OnPropertyChanged(nameof(SourceText));
			}
		}

		[SourceModelProperty("MatchText", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string MatchText
		{
			get
			{
				return Flyweight.Get<string>(_matchText, When.MatchText, new VariableStringToStringConverter());
			}
			set
			{
				this._matchText = value;
				this.OnPropertyChanged(nameof(MatchText));
			}
		}
	}
}
