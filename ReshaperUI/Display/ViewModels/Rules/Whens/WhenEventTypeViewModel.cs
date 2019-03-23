using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Event Type"), Export(typeof(WhenViewModel<WhenEventType>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenEventTypeViewModel : WhenViewModel<WhenEventType>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _type;

		public IEnumerable<string> EventTypes
		{
			get
			{
				return (Enum.GetValues(typeof(EventType)) as EventType[]).Select(eventType => (string)(new EnumToStringConverter().Convert(eventType, typeof(EventType))));
			}
		}

		[SourceModelProperty("Type", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string Type
		{
			get
			{
				return Flyweight.Get<string>(_type, When.Type, new EnumToStringConverter());
			}
			set
			{
				this._type = value;
				this.OnPropertyChanged(nameof(Type));
			}
		}
	}
}
