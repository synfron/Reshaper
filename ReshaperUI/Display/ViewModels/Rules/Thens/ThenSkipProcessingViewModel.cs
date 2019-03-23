using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Skip Processing"), Export(typeof(ThenViewModel<ThenSkipProcessing>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenSkipProcessingViewModel : ThenViewModel<ThenSkipProcessing>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenSkipProcessing then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
