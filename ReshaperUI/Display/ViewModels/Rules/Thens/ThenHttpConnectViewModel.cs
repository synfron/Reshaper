using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;
using ReshaperUI.Attributes;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("HTTP Connect"), Export(typeof(ThenViewModel<ThenHttpConnect>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenHttpConnectViewModel : ThenViewModel<ThenHttpConnect>, IHttpRuleOperationModel
	{
		private bool _overrideCurrentConnection;

		[SourceModelProperty("OverrideCurrentConnection")]
		public bool OverrideCurrentConnection
		{
			get
			{
				return Flyweight.Get<bool>(_overrideCurrentConnection, Then.OverrideCurrentConnection);
			}
			set
			{
				this._overrideCurrentConnection = value;
				this.OnPropertyChanged(nameof(OverrideCurrentConnection));
			}
		}
	}
}
