using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Send Data"), Export(typeof(ThenViewModel<ThenSendData>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenSendDataViewModel : ThenViewModel<ThenSendData>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenSendData then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
