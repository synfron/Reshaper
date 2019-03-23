using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Delimit Text"), Export(typeof(ThenViewModel<ThenDelimitText>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenDelimitTextViewModel : ThenViewModel<ThenDelimitText>, ITextRuleOperationModel
	{
		public override void SetModels(RuleViewModel rule, ThenDelimitText then = null)
		{
			base.SetModels(rule, then);

			if (!IsSaved)
			{
				SaveCommand.Execute(this);
			}
		}
	}
}
