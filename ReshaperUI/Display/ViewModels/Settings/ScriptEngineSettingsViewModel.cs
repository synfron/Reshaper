using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ReshaperScript.Providers;
using ReshaperScript.Settings;
using ReshaperUI.Attributes;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class ScriptEngineSettingsViewModel : SourceModelViewModel
	{
		private int? _scriptTimeoutInSeconds;
		private int? _poolEngineExpirationInMinutes;
		private int? _poolEngineExpirationUseCount;
		private int? _maxEnginesInPool;
		private SourceModelSaveCommand<IScriptEngineSettings> _saveCommand;

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<IScriptEngineSettings>();
				}
				return _saveCommand;
			}
		}

		[SourceModelProperty("MaxEnginesInPool")]
		[Required(ErrorMessage = "'Max Engines in Pool' is required.")]
		[Range(0, int.MaxValue)]
		public int MaxEnginesInPool
		{
			get
			{
				return Flyweight.Get<int>(_maxEnginesInPool, ScriptEngineSettings.MaxEnginesInPool);
			}
			set
			{
				this._maxEnginesInPool = value;
				this.OnPropertyChanged(nameof(MaxEnginesInPool));
			}
		}

		[SourceModelProperty("PoolEngineExpirationInMinutes")]
		[Required(ErrorMessage = "'Pool Engine Idle Expiration' is required.")]
		[Range(0, int.MaxValue)]
		public int PoolEngineExpirationInMinutes
		{
			get
			{
				return Flyweight.Get<int>(_poolEngineExpirationInMinutes, ScriptEngineSettings.PoolEngineExpirationInMinutes);
			}
			set
			{
				this._poolEngineExpirationInMinutes = value;
				this.OnPropertyChanged(nameof(PoolEngineExpirationInMinutes));
			}
		}

		[SourceModelProperty("PoolEngineExpirationUseCount")]
		[Required(ErrorMessage = "'Pool Engine Expires After N Uses' is required.")]
		[Range(0, int.MaxValue)]
		public int PoolEngineExpirationUseCount
		{
			get
			{
				return Flyweight.Get<int>(_poolEngineExpirationUseCount, ScriptEngineSettings.PoolEngineExpirationUseCount);
			}
			set
			{
				this._poolEngineExpirationUseCount = value;
				this.OnPropertyChanged(nameof(PoolEngineExpirationUseCount));
			}
		}

		[SourceModelProperty("ScriptTimeoutInSeconds")]
		[Required(ErrorMessage = "'Script Run Timeout' is required.")]
		[Range(0, int.MaxValue)]
		public int ScriptTimeoutInSeconds
		{
			get
			{
				return Flyweight.Get<int>(_scriptTimeoutInSeconds, ScriptEngineSettings.ScriptTimeoutInSeconds);
			}
			set
			{
				this._scriptTimeoutInSeconds = value;
				this.OnPropertyChanged(nameof(ScriptTimeoutInSeconds));
			}
		}


		[SourceModel]
		public IScriptEngineSettings ScriptEngineSettings { get; private set; }

		protected override string PartialDisplayName
		{
			get
			{
				return "Script Engine";
			}
		}

		public ScriptEngineSettingsViewModel()
		{
			ScriptEngineSettingsProvider scriptEngineSettingsProvider = new ScriptEngineSettingsProvider();
			ScriptEngineSettings = scriptEngineSettingsProvider.GetInstance();
		}
	}
}
