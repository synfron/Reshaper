using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Vars;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Set Variable"), Export(typeof(ThenViewModel<ThenSetVariable>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenSetVariableViewModel : ThenSetViewModel<ThenSetVariable>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _targetSource;
		private string _variableName;
		private string _destinationIdentifier;
		private string _destinationMessageValueType;

		public IEnumerable<string> VariableSources
		{
			get
			{
				return (Enum.GetValues(typeof(VariableSource)) as VariableSource[]).Select(variableSource => (string)(new EnumToStringConverter().Convert(variableSource, typeof(VariableSource))));
			}
		}

		[SourceModelProperty("TargetSource", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string TargetSource
		{
			get
			{
				return Flyweight.Get<string>(_targetSource, Then.TargetSource, new EnumToStringConverter());
			}
			set
			{
				this._targetSource = value;
				this.OnPropertyChanged(nameof(TargetSource));
			}
		}

		[SourceModelProperty("VariableName", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[Required(ErrorMessage = "'Variable Name' is required")]
		public string VariableName
		{
			get
			{
				return Flyweight.Get<string>(_variableName, Then.VariableName, new VariableStringToStringConverter());
			}
			set
			{
				this._variableName = value;
				this.OnPropertyChanged(nameof(VariableName));
			}
		}

		[SourceModelProperty("DestinationMessageValueType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		[Required(ErrorMessage = "'Destination Message Value Type' is required.")]
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
		[RequiredDependent(ErrorMessage = "'Destination Identifier' is required.", DependentProperty = "DestinationMessageValueType", DependentValue = "JSON")]
		[RequiredDependent(ErrorMessage = "'Destination Identifier' is required.", DependentProperty = "DestinationMessageValueType", DependentValue = "XML")]
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
