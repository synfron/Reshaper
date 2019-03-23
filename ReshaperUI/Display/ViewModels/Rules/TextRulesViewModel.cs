using ReshaperUI.Display.ViewModels.EventViews;
using ReshaperCore.Providers;
using ReshaperCore.Rules;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public class TextRulesViewModel : RulesViewModel, IEventViewModel
	{
		private ITextRulesRegistry _rulesRegistry;

		public override string DisplayName
		{
			get
			{
				return "Text Rules";
			}
		}

		protected override IRulesRegistry RulesRegistry
		{
			get
			{
				if (_rulesRegistry == null)
				{
					TextRulesRegistryProvider textRulesRegistryProvider = new TextRulesRegistryProvider();
					_rulesRegistry = textRulesRegistryProvider.GetInstance();
				}
				return _rulesRegistry;
			}
		}

		public TextRulesViewModel()
		{
			RulesRegistry.RulesListChanged += OnRulesListChanged;
		}

		protected override void UpdateRulesList()
		{
			Rules.Clear();
			Rules.Add(new TextRuleViewModel().Init());
			foreach (Rule rule in RulesRegistry.GetRules())
			{
				Rules.Add(new TextRuleViewModel(rule).Init());
			}
		}
	}
}
