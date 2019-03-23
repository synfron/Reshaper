using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Whens;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Is System Proxy"), Export(typeof(WhenViewModel<WhenIsSystemProxy>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenIsSystemProxyViewModel : WhenViewModel<WhenIsSystemProxy>, IHttpRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, WhenIsSystemProxy when = null)
		{
			base.SetModels(rule, when);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
