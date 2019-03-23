using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Delimit HTTP"), Export(typeof(ThenViewModel<ThenDelimitHttp>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenDelimitHttpViewModel : ThenViewModel<ThenDelimitHttp>, IHttpRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenDelimitHttp then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
