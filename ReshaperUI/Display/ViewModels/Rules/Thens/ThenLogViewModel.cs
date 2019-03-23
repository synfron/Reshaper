using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using ReshaperCore.Rules.Thens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Log"), Export(typeof(ThenViewModel<ThenLog>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenLogViewModel : ThenViewModel<ThenLog>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _text;

		[SourceModelProperty("Text", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		[Required(ErrorMessage = "'Text' is required.")]
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
	}
}
