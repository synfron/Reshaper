using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Broadcast"), Export(typeof(ThenViewModel<ThenBroadcast>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenBroadcastViewModel : ThenViewModel<ThenBroadcast>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenBroadcast then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
