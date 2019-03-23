using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Messages;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Vars;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Add Message"), Export(typeof(ThenViewModel<ThenAddMessage>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenAddMessageViewModel : ThenViewModel<ThenAddMessage>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _direction;
		private string _delimiter;
		private string _messageText;
		private bool? _insertAtBeginning;

		public IEnumerable<string> DataDirections
		{
			get
			{
				return (Enum.GetValues(typeof(DataDirection)) as DataDirection[]).Select(dataDirection => (string)(new EnumToStringConverter().Convert(dataDirection, typeof(DataDirection))));
			}
		}

		public IEnumerable<string> VariableSources
		{
			get
			{
				return (Enum.GetValues(typeof(VariableSource)) as VariableSource[]).Select(variableSource => (string)(new EnumToStringConverter().Convert(variableSource, typeof(VariableSource))));
			}
		}

		[SourceModelProperty("Direction", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string Direction
		{
			get
			{
				return Flyweight.Get<string>(_direction, Then.Direction, new EnumToStringConverter());
			}
			set
			{
				this._direction = value;
				this.OnPropertyChanged(nameof(Direction));
			}
		}

		[SourceModelProperty("Delimiter")]
		public string Delimiter
		{
			get
			{
				return Flyweight.Get<string>(_delimiter, Then.Delimiter);
			}
			set
			{
				this._delimiter = value;
				this.OnPropertyChanged(nameof(Delimiter));
			}
		}

		[SourceModelProperty("MessageText", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string MessageText
		{
			get
			{
				return Flyweight.Get<string>(_messageText, Then.MessageText, new VariableStringToStringConverter());
			}
			set
			{
				this._messageText = value;
				this.OnPropertyChanged(nameof(MessageText));
			}
		}

		[SourceModelProperty("InsertAtBeginning")]
		public bool InsertAtBeginning
		{
			get
			{
				return Flyweight.Get<bool>(_insertAtBeginning, Then.InsertAtBeginning);
			}
			set
			{
				this._insertAtBeginning = value;
				this.OnPropertyChanged(nameof(InsertAtBeginning));
			}
		}
	}
}
