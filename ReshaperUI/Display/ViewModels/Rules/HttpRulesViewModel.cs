using ReshaperUI.Display.ViewModels.EventViews;
using ReshaperCore.Providers;
using ReshaperCore.Rules;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public class HttpRulesViewModel : RulesViewModel, IEventViewModel
	{
		private IHttpRulesRegistry _rulesRegistry;

		public override string DisplayName
		{
			get
			{
				return "HTTP Rules";
			}
		}

		protected override IRulesRegistry RulesRegistry
		{
			get
			{
				if (_rulesRegistry == null)
				{
					HttpRulesRegistryProvider httpRulesRegistryProvider = new HttpRulesRegistryProvider();
					_rulesRegistry = httpRulesRegistryProvider.GetInstance();
				}
				return _rulesRegistry;
			}
		}

		public HttpRulesViewModel()
		{
			RulesRegistry.RulesListChanged += OnRulesListChanged;
		}

		protected override void UpdateRulesList()
		{
			Rules.Clear();
			Rules.Add(new HttpRuleViewModel().Init());
			foreach (Rule rule in RulesRegistry.GetRules())
			{
				Rules.Add(new HttpRuleViewModel(rule).Init());
			}
		}
	}
}
