using Newtonsoft.Json;

namespace ReshaperCore.Settings
{
	[JsonObject("GeneralSettings")]
	public class GeneralSettings : SettingsStore, IGeneralSettings
	{
		private bool? _autoUpdateContentLength;
		private bool? _ignoreContentLength;

		public bool AutoUpdateContentLength
		{
			get
			{
				if (_autoUpdateContentLength == null)
				{
					_autoUpdateContentLength = GetValue<bool>(nameof(AutoUpdateContentLength));
				}
				return _autoUpdateContentLength.Value;
			}
			set
			{
				_autoUpdateContentLength = value;
				SetValue(nameof(AutoUpdateContentLength), value);
			}
		}

		public bool IgnoreContentLength
		{
			get
			{
				if (_ignoreContentLength == null)
				{
					_ignoreContentLength = GetValue<bool>(nameof(IgnoreContentLength));
				}
				return _ignoreContentLength.Value;
			}
			set
			{
				_ignoreContentLength = value;
				SetValue(nameof(IgnoreContentLength), value);
			}
		}

		protected override T GetDefaultValue<T>(string propertyName)
		{
			object returnVal = null;
			switch (propertyName)
			{
				case "AutoUpdateContentLength":
					returnVal = true;
					break;
				case "IgnoreContentLength":
					returnVal = false;
					break;
			}
			return (returnVal == null) ? default(T) : (T)returnVal;
		}
	}
}
