using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Has Entity"), Export(typeof(WhenViewModel<WhenHasEntity>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenHasEntityViewModel : WhenViewModel<WhenHasEntity>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private static IEnumerable<string> _settableMessageValueTypes;
		private string _messageValue;
		private string _identifier;
		private string _messageValueType;

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

		[SourceModelProperty("MessageValue", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
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

		[SourceModelProperty("MessageValueType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Message Value Type' is required.", DependentProperty = "MessageValue", DependentValue = "HTTP Body")]
		[RequiredDependent(ErrorMessage = "'Message Value Type' is required.", DependentProperty = "MessageValue", DependentValue = "Message")]
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
		[RequiredDependent(ErrorMessage = "'Message Value Identifier' is required.", DependentProperty = "MessageValue", DependentValue = "HTTP Header")]
		[RequiredDependent(ErrorMessage = "'Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"HTTP Body\",\"MessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"HTTP Body\",\"MessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"Message\",\"MessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"MessageValue\":\"Message\",\"MessageValueType\":\"XML\"}")]
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
	}
}
