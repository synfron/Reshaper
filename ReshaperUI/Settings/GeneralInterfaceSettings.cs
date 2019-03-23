using Newtonsoft.Json;
using ReshaperCore.Settings;

namespace ReshaperUI.Settings
{
	[JsonObject("GeneralSettings")]
	public class GeneralInterfaceSettings : GeneralSettings, IGeneralInterfaceSettings
	{
		private bool? _autoTruncateMessages;
		private int? _truncatedMessageMaxSize;
		private bool? _limitEventBufferSize;
		private int? _maxEventBufferSize;

		public bool AutoTruncateMessages
		{
			get
			{
				if (_autoTruncateMessages == null)
				{
					_autoTruncateMessages = GetValue<bool>(nameof(AutoTruncateMessages));
				}
				return _autoTruncateMessages.Value;
			}
			set
			{
				_autoTruncateMessages = value;
				SetValue(nameof(AutoTruncateMessages), value);
			}
		}

		public int TruncatedMessageMaxSize
		{
			get
			{
				if (_truncatedMessageMaxSize == null)
				{
					_truncatedMessageMaxSize = GetValue<int>(nameof(TruncatedMessageMaxSize));
				}
				return _truncatedMessageMaxSize.Value;
			}
			set
			{
				_truncatedMessageMaxSize = value;
				SetValue(nameof(TruncatedMessageMaxSize), value);
			}
		}

		public bool LimitEventBufferSize
		{
			get
			{
				if (_limitEventBufferSize == null)
				{
					_limitEventBufferSize = GetValue<bool>(nameof(LimitEventBufferSize));
				}
				return _limitEventBufferSize.Value;
			}
			set
			{
				_limitEventBufferSize = value;
				SetValue(nameof(LimitEventBufferSize), value);
			}
		}

		public int MaxEventBufferSize
		{
			get
			{
				if (_maxEventBufferSize == null)
				{
					_maxEventBufferSize = GetValue<int>(nameof(MaxEventBufferSize));
				}
				return _maxEventBufferSize.Value;
			}
			set
			{
				_maxEventBufferSize = value;
				SetValue(nameof(MaxEventBufferSize), value);
			}
		}

		protected override T GetDefaultValue<T>(string propertyName)
		{
			object returnVal = null;
			switch (propertyName)
			{
				case "AutoTruncateMessages":
					returnVal = true;
					break;
				case "TruncatedMessageMaxSize":
					returnVal = 75000;
					break;
				case "LimitEventBufferSize":
					returnVal = true;
					break;
				case "MaxEventBufferSize":
					returnVal = 200;
					break;
			}
			return (T)(returnVal ?? base.GetDefaultValue<T>(propertyName));
		}
	}
}
