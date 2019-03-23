using System.Collections.ObjectModel;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperScript.Core;
using ReshaperScript.Providers;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class ScriptListViewModel : ObservableViewModel
	{
		private readonly ObservableCollection<ScriptViewModel> _scripts = new ObservableCollection<ScriptViewModel>();
		private ScriptViewModel _selectedScript;
		private readonly IScriptRegistry _scriptRegistry;

		public ObservableCollection<ScriptViewModel> Scripts
		{
			get
			{
				return _scripts;
			}
		}

		public ScriptViewModel SelectedScript
		{
			get
			{
				return _selectedScript;
			}
			set
			{
				_selectedScript = value;
				OnPropertyChanged(nameof(SelectedScript));
			}
		}

		public ScriptListViewModel()
		{
			ScriptRegistryProvider scriptRegistryProvider = new ScriptRegistryProvider();
			_scriptRegistry = scriptRegistryProvider.GetInstance();

			UpdateScriptList();
			_scriptRegistry.ScriptsListChanged += OnScriptsCollectionChanged;
		}

		private void OnScriptsCollectionChanged()
		{
			UpdateScriptList();
		}

		private void UpdateScriptList()
		{
			Scripts.Clear();
			Scripts.Add(new ScriptViewModel());
			foreach (Script script in _scriptRegistry.Scripts)
			{
				Scripts.Add(new ScriptViewModel(script));
			}
		}
	}
}
