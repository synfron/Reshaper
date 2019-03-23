using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Disconnect"), Export(typeof(ThenViewModel<ThenDisconnect>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenDisconnectViewModel : ThenViewModel<ThenDisconnect>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenDisconnect then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
