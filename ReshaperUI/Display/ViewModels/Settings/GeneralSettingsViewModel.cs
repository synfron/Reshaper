using System.Windows.Input;
using ReshaperUI.Attributes;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Providers;
using ReshaperUI.Settings;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class GeneralSettingsViewModel : SourceModelViewModel
	{
		private bool? _autoUpdateContentLength;
		private bool? _ignoreContentLength;
		private bool? _autoTruncateMessages;
		private int? _truncatedMessageMaxSize;
		private bool? _limitEventBufferSize;
		private int? _maxEventBufferSize;
		private SourceModelSaveCommand<IGeneralInterfaceSettings> _saveCommand;

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<IGeneralInterfaceSettings>();
				}
				return _saveCommand;
			}
		}

		[SourceModel]
		public IGeneralInterfaceSettings GeneralSettings { get; private set; }

		protected override string PartialDisplayName
		{
			get
			{
				return "General";
			}
		}

		[SourceModelProperty("AutoUpdateContentLength")]
		public bool AutoUpdateContentLength
		{
			get
			{
				return Flyweight.Get<bool>(_autoUpdateContentLength, GeneralSettings.AutoUpdateContentLength);
			}
			set
			{
				this._autoUpdateContentLength = value;
				this.OnPropertyChanged(nameof(AutoUpdateContentLength));
			}
		}

		[SourceModelProperty("IgnoreContentLength")]
		public bool IgnoreContentLength
		{
			get
			{
				return Flyweight.Get<bool>(_ignoreContentLength, GeneralSettings.IgnoreContentLength);
			}
			set
			{
				this._ignoreContentLength = value;
				this.OnPropertyChanged(nameof(IgnoreContentLength));
			}
		}

		[SourceModelProperty("AutoTruncateMessages")]
		public bool AutoTruncateMessages
		{
			get
			{
				return Flyweight.Get<bool>(_autoTruncateMessages, GeneralSettings.AutoTruncateMessages);
			}
			set
			{
				this._autoTruncateMessages = value;
				this.OnPropertyChanged(nameof(AutoTruncateMessages));
			}
		}

		[SourceModelProperty("TruncatedMessageMaxSize")]
		[RequiredDependent(ErrorMessage = "'Truncated Message Max Size' is required.", DependentProperty = "AutoTruncateMessages", DependentValue = true)]
		[RangeDependent(0, int.MaxValue, ErrorMessage = "Unsupported value for 'Truncated Message Max Size'", DependentProperty = "AutoTruncateMessages", DependentValue = true)]
		public int TruncatedMessageMaxSize
		{
			get
			{
				return Flyweight.Get<int>(_truncatedMessageMaxSize, GeneralSettings.TruncatedMessageMaxSize);
			}
			set
			{
				this._truncatedMessageMaxSize = value;
				this.OnPropertyChanged(nameof(TruncatedMessageMaxSize));
			}
		}

		[SourceModelProperty("LimitEventBufferSize")]
		public bool LimitEventBufferSize
		{
			get
			{
				return Flyweight.Get<bool>(_limitEventBufferSize, GeneralSettings.LimitEventBufferSize);
			}
			set
			{
				this._limitEventBufferSize = value;
				this.OnPropertyChanged(nameof(LimitEventBufferSize));
			}
		}

		[SourceModelProperty("MaxEventBufferSize")]
		[RequiredDependent(ErrorMessage = "'Max Event Buffer Size' is required.", DependentProperty = "LimitEventBufferSize", DependentValue = true)]
		[RangeDependent(0, int.MaxValue, ErrorMessage = "Unsupported value for 'Max Event Buffer Size'", DependentProperty = "LimitEventBufferSize", DependentValue = true)]
		public int MaxEventBufferSize
		{
			get
			{
				return Flyweight.Get<int>(_maxEventBufferSize, GeneralSettings.MaxEventBufferSize);
			}
			set
			{
				this._maxEventBufferSize = value;
				this.OnPropertyChanged(nameof(MaxEventBufferSize));
			}
		}

		public GeneralSettingsViewModel()
		{
			GeneralInterfaceSettingsProvider generalInterfaceSettingsProvider = new GeneralInterfaceSettingsProvider();
			GeneralSettings = generalInterfaceSettingsProvider.GetInstance();
		}
	}
}
