using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Messages;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Event Direction"), Export(typeof(WhenViewModel<WhenEventDirection>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenEventDirectionViewModel : WhenViewModel<WhenEventDirection>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _direction;

		public IEnumerable<string> DataDirections
		{
			get
			{
				return (Enum.GetValues(typeof(DataDirection)) as DataDirection[]).Select(dataDirection => (string)(new EnumToStringConverter().Convert(dataDirection, typeof(DataDirection))));
			}
		}

		[SourceModelProperty("Direction", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string Direction
		{
			get
			{
				return Flyweight.Get<string>(_direction, When.Direction, new EnumToStringConverter());
			}
			set
			{
				this._direction = value;
				this.OnPropertyChanged(nameof(Direction));
			}
		}
	}
}
