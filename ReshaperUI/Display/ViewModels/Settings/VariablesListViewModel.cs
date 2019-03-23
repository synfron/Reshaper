using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperCore.Providers;
using ReshaperCore.Vars;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class VariablesListViewModel : ObservableViewModel
	{
		private readonly ObservableCollection<VariableViewModel> _variables = new ObservableCollection<VariableViewModel>();
		private VariableViewModel _selectedVariable;
		private Variables _globalVariables;

		public ObservableCollection<VariableViewModel> Variables
		{
			get
			{
				return _variables;
			}
		}

		public VariableViewModel SelectedVariable
		{
			get
			{
				return _selectedVariable;
			}
			set
			{
				_selectedVariable = value;
				OnPropertyChanged(nameof(SelectedVariable));
			}
		}

		public VariablesListViewModel()
		{
			SelfProvider selfProvider = new SelfProvider();

			_globalVariables = selfProvider.GetInstance().Variables;
			UpdateVariablesList();
			_globalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
		}

		private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				UpdateVariablesList();
			}
			else
			{
				if (e.OldItems != null)
				{
					foreach (string name in e.OldItems.OfType<string>())
					{
						VariableViewModel model = Variables.FirstOrDefault(variableModel => variableModel.VariableName == name);
						model.Dispose();
						Variables.Remove(model);
					}
				}
				if (e.NewItems != null)
				{
					foreach (string name in e.NewItems.OfType<string>())
					{
						Variables.Add(new VariableViewModel(name));
					}
				}
			}
		}

		private void UpdateVariablesList()
		{
			foreach (VariableViewModel model in Variables)
			{
				model.Dispose();
			}
			Variables.Clear();
			Variables.Add(new VariableViewModel());
			foreach (string name in _globalVariables.VariableNames)
			{
				Variables.Add(new VariableViewModel(name));
			}
		}
	}
}
