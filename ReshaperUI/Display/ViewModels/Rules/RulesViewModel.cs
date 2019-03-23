using System.Collections.ObjectModel;
using System.Windows.Input;
using ReshaperCore.Rules;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public interface IRulesViewModel
	{

	}

	public abstract class RulesViewModel : ObservableViewModel, IRulesViewModel
	{
		private readonly ObservableCollection<RuleViewModel> _rules = new ObservableCollection<RuleViewModel>();
		private RuleViewModel _selectedRule;
		private RelayCommand<Rule> _moveUpCommand;
		private RelayCommand<Rule> _moveDownCommand;
		private RelayCommand<Rule> _deleteCommand;


		protected abstract IRulesRegistry RulesRegistry { get; }


		public ICommand MoveUpCommand
		{
			get
			{
				if (_moveUpCommand == null)
				{
					_moveUpCommand = new RelayCommand<Rule>(RulesRegistry.MovePrevious, RulesRegistry.CanMovePrevious);
				}
				return _moveUpCommand;
			}
		}

		public ICommand MoveDownCommand
		{
			get
			{
				if (_moveDownCommand == null)
				{
					_moveDownCommand = new RelayCommand<Rule>(RulesRegistry.MoveNext, RulesRegistry.CanMoveNext);
				}
				return _moveDownCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand<Rule>(RulesRegistry.DeleteRule, RulesRegistry.CanDelete);
				}
				return _deleteCommand;
			}
		}

		public RuleViewModel SelectedRule
		{
			get
			{
				return _selectedRule;
			}
			set
			{
				_selectedRule = value;
				OnPropertyChanged(nameof(SelectedRule));
			}
		}

		public ObservableCollection<RuleViewModel> Rules
		{
			get
			{
				return _rules;
			}
		}

		public abstract string DisplayName
		{
			get;
		}

		public RulesViewModel()
		{
			UpdateRulesList();
		}

		protected void OnRulesListChanged()
		{
			UpdateRulesList();
		}

		protected abstract void UpdateRulesList();
	}
}
