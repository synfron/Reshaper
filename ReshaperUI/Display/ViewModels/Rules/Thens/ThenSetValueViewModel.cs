using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Rules.Thens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Set Value"), Export(typeof(ThenViewModel<ThenSetValue>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenSetValueViewModel : ThenSetViewModel<ThenSetValue>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private static IEnumerable<string> _settableMessageValues;
		private string _destinationMessageValue;
		private string _destinationIdentifier;
		private string _destinationMessageValueType;

		public IEnumerable<string> SettableMessageValues
		{
			get
			{
				if (_settableMessageValues == null)
				{
					if (RuleModel is HttpRuleViewModel)
					{
						_settableMessageValues = (new[] { MessageValue.DataDirection, MessageValue.HttpBody, MessageValue.HttpHeader, MessageValue.HttpHeaders, MessageValue.HttpMethod, MessageValue.HttpRequestUri, MessageValue.HttpStatusCode, MessageValue.HttpStatusLine, MessageValue.HttpStatusMessage, MessageValue.HttpVersion, MessageValue.Message }).Select(packeValue => (string)(new EnumToStringConverter().Convert(packeValue, typeof(MessageValue))));
					}
					else
					{
						_settableMessageValues = (new[] { MessageValue.DataDirection, MessageValue.Message }).Select(packeValue => (string)(new EnumToStringConverter().Convert(packeValue, typeof(MessageValue))));
					}
				}
				return _settableMessageValues;
			}
		}

		[SourceModelProperty("DestinationMessageValue", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string DestinationMessageValue
		{
			get
			{
				return Flyweight.Get<string>(_destinationMessageValue, Then.DestinationMessageValue, new EnumToStringConverter());
			}
			set
			{
				this._destinationMessageValue = value;
				this.OnPropertyChanged(nameof(DestinationMessageValue));
			}
		}

		[SourceModelProperty("DestinationMessageValueType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Type' is required.", DependentProperty = "DestinationMessageValue", DependentValue = "HTTP Body")]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Type' is required.", DependentProperty = "DestinationMessageValue", DependentValue = "Message")]
		public string DestinationMessageValueType
		{
			get
			{
				return Flyweight.Get<string>(_destinationMessageValueType, Then.DestinationMessageValueType, new EnumToStringConverter());
			}
			set
			{
				this._destinationMessageValueType = value;
				this.OnPropertyChanged(nameof(DestinationMessageValueType));
			}
		}

		[SourceModelProperty("DestinationIdentifier", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Identifier' is required.", DependentProperty = "DestinationMessageValue", DependentValue = "HTTP Header")]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"DestinationMessageValue\":\"HTTP Body\",\"DestinationMessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"DestinationMessageValue\":\"HTTP Body\",\"DestinationMessageValueType\":\"JSON\"}")]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"DestinationMessageValue\":\"Message\",\"DestinationMessageValueType\":\"XML\"}")]
		[RequiredDependent(ErrorMessage = "'Destination Message Value Identifier' is required.", JsonDependentPropertyValuePairs = "{\"DestinationMessageValue\":\"Message\",\"DestinationMessageValueType\":\"JSON\"}")]
		public string DestinationIdentifier
		{
			get
			{
				return Flyweight.Get<string>(_destinationIdentifier, Then.DestinationIdentifier, new VariableStringToStringConverter());
			}
			set
			{
				this._destinationIdentifier = value;
				this.OnPropertyChanged(nameof(DestinationIdentifier));
			}
		}
	}
}
