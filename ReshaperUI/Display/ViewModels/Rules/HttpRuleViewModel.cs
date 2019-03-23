using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows.Input;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperCore.Utils.Extensions;
using ReshaperUI.Display.ViewModels.Rules.Thens;
using ReshaperUI.Display.ViewModels.Rules.Whens;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public class HttpRuleViewModel : RuleViewModel
	{
		private static List<string> _whenNames;
		private static List<string> _thenNames;
		private SourceModelSaveCommand<Rule> _saveCommand;
		private readonly IHttpRulesRegistry _httpRulesRegistry;

		public override ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<Rule>(_httpRulesRegistry.AddRule, CanSave);
				}
				return _saveCommand;
			}
		}

		public override List<string> WhenNames
		{
			get
			{
				return _whenNames;
			}
		}

		public override List<string> ThenNames
		{
			get
			{
				return _thenNames;
			}
		}

		static HttpRuleViewModel()
		{
			CompositionContainerProvider compositionContainerProvider = new CompositionContainerProvider();
			CompositionContainer container = compositionContainerProvider.GetInstance();

			_whenNames = container.GetDistinctExportsTypes(typeof(IHttpRuleOperationModel), typeof(WhenViewModel)).GetAttributes<DescriptionAttribute>().Select(attr => attr.Description).ToList();
			_thenNames = container.GetDistinctExportsTypes(typeof(IHttpRuleOperationModel), typeof(ThenViewModel)).GetAttributes<DescriptionAttribute>().Select(attr => attr.Description).ToList();
		}

		public HttpRuleViewModel(Rule rule = null) : base(rule)
		{
			HttpRulesRegistryProvider httpRulesRegistryProvider = new HttpRulesRegistryProvider();
			_httpRulesRegistry = httpRulesRegistryProvider.GetInstance();
		}
	}
}
