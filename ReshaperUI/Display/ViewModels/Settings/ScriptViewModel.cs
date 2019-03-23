using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ReshaperScript.Core;
using ReshaperScript.Providers;
using ReshaperUI.Attributes;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class ScriptViewModel : SourceModelViewModel
	{
		private SourceModelSaveCommand<Script> _saveCommand;
		private string _scriptName;
		private string _scriptText;
		private bool? _isStaticScript;
		private RelayCommand _deleteCommand;
		private readonly IScriptRegistry _scriptRegistry;

		[SourceModel]
		public Script Script { get; private set; }

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<Script>(_scriptRegistry.AddScript, CanSave);
				}
				return _saveCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(() => _scriptRegistry.DeleteScript(Script), delegate () { return !IsNew; });
				}
				return _deleteCommand;
			}
		}

		protected override string PartialDisplayName
		{
			get
			{
				return (IsNew) ? "New..." : ScriptName;
			}
		}

		[SourceModelProperty("Name")]
		[Required(ErrorMessage = "'Script Name' is required.")]
		public string ScriptName
		{
			get
			{
				return Flyweight.Get<string>(_scriptName, Script?.Name);
			}
			set
			{
				this._scriptName = value;
				this.OnPropertyChanged(nameof(ScriptName));
				this.OnPropertyChanged(nameof(DisplayName));
			}
		}

		[SourceModelProperty("Text")]
		public string ScriptText
		{
			get
			{
				return Flyweight.Get<string>(_scriptText, Script.Text);
			}
			set
			{
				this._scriptText = value;
				this.OnPropertyChanged(nameof(ScriptText));
			}
		}

		[SourceModelProperty("IsStaticScript")]
		public bool IsStaticScript
		{
			get
			{
				return Flyweight.Get<bool>(_isStaticScript, Script.IsStaticScript);
			}
			set
			{
				this._isStaticScript = value;
				this.OnPropertyChanged(nameof(IsStaticScript));
			}
		}

		public ScriptViewModel(Script script = null)
		{

			ScriptRegistryProvider scriptRegistryProvider = new ScriptRegistryProvider();
			_scriptRegistry = scriptRegistryProvider.GetInstance();

			this.Script = script ?? new Script();
			IsNew = script == null;
		}
	}
}
