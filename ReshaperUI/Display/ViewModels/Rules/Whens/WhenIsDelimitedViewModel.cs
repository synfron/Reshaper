using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Whens;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Is Delimited"), Export(typeof(WhenViewModel<WhenIsDelimited>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenIsDelimitedViewModel : WhenViewModel<WhenIsDelimited>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, WhenIsDelimited when = null)
		{
			base.SetModels(rule, when);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
