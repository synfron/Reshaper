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
using ReshaperUI.Providers;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public class TextRuleViewModel : RuleViewModel
	{
		private SourceModelSaveCommand<Rule> _saveCommand;
		private static readonly List<string> _whenNames;
		private static readonly List<string> _thenNames;
		private readonly ITextRulesRegistry _textRulesRegistry;

		public override ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<Rule>(_textRulesRegistry.AddRule, CanSave);
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

		static TextRuleViewModel()
		{
			CompositionContainerProvider compositionContainerProvider = new CompositionContainerProvider();
			CompositionContainer container = compositionContainerProvider.GetInstance();

			_whenNames = container.GetDistinctExportsTypes(typeof(ITextRuleOperationModel), typeof(WhenViewModel)).GetAttributes<DescriptionAttribute>().Select(attr => attr.Description).ToList();
			_thenNames = container.GetDistinctExportsTypes(typeof(ITextRuleOperationModel), typeof(ThenViewModel)).GetAttributes<DescriptionAttribute>().Select(attr => attr.Description).ToList();
		}

		public TextRuleViewModel(Rule rule = null) : base(rule)
		{
			TextRulesRegistryProvider textRulesRegistryProvider = new TextRulesRegistryProvider();
			_textRulesRegistry = textRulesRegistryProvider.GetInstance();
		}
	}
}
