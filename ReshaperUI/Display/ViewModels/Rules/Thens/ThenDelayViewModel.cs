using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Delay"), Export(typeof(ThenViewModel<ThenDelay>))]
	public class ThenDelayViewModel : ThenViewModel<ThenDelay>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _delay;

		[SourceModelProperty("Delay", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[Required(ErrorMessage = "'Delay' is required.")]
		public string Delay
		{
			get
			{
				return Flyweight.Get<string>(_delay, Then.Delay, new VariableStringToStringConverter());
			}
			set
			{
				this._delay = value;
				this.OnPropertyChanged(nameof(Delay));
			}
		}
	}
}
