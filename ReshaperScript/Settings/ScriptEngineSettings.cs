using Newtonsoft.Json;

namespace ReshaperScript.Settings
{
	[JsonObject("ScriptEngineSettings")]
	public class ScriptEngineSettings : ReshaperCore.Settings.SettingsStore, IScriptEngineSettings
	{
		private int? _scriptTimeoutInSeconds;
		private int? _poolEngineExpirationInMinutes;
		private int? _poolEngineExpirationUseCount;
		private int? _maxEnginesInPool;

		public int ScriptTimeoutInSeconds
		{
			get
			{
				if (_scriptTimeoutInSeconds == null)
				{
					_scriptTimeoutInSeconds = GetValue<int>(nameof(ScriptTimeoutInSeconds));
				}
				return _scriptTimeoutInSeconds.Value;
			}
			set
			{
				_scriptTimeoutInSeconds = value;
				SetValue(nameof(ScriptTimeoutInSeconds), value);
			}
		}

		public int PoolEngineExpirationInMinutes
		{
			get
			{
				if (_poolEngineExpirationInMinutes == null)
				{
					_poolEngineExpirationInMinutes = GetValue<int>(nameof(PoolEngineExpirationInMinutes));
				}
				return _poolEngineExpirationInMinutes.Value;
			}
			set
			{
				_poolEngineExpirationInMinutes = value;
				SetValue(nameof(PoolEngineExpirationInMinutes), value);
			}
		}

		public int PoolEngineExpirationUseCount
		{
			get
			{
				if (_poolEngineExpirationUseCount == null)
				{
					_poolEngineExpirationUseCount = GetValue<int>(nameof(PoolEngineExpirationUseCount));
				}
				return _poolEngineExpirationUseCount.Value;
			}
			set
			{
				_poolEngineExpirationUseCount = value;
				SetValue(nameof(PoolEngineExpirationInMinutes), value);
			}
		}

		public int MaxEnginesInPool
		{
			get
			{
				if (_maxEnginesInPool == null)
				{
					_maxEnginesInPool = GetValue<int>(nameof(MaxEnginesInPool));
				}
				return _maxEnginesInPool.Value;
			}
			set
			{
				_maxEnginesInPool = value;
				SetValue(nameof(MaxEnginesInPool), value);
			}
		}

		protected override T GetDefaultValue<T>(string propertyName)
		{
			object returnVal = null;
			switch (propertyName)
			{
				case "ScriptTimeoutInSeconds":
					returnVal = 60;
					break;
				case "PoolEngineExpirationInMinutes":
					returnVal = 60;
					break;
				case "PoolEngineExpirationUseCount":
					returnVal = 100;
					break;
				case "MaxEnginesInPool":
					returnVal = 5;
					break;
			}
			return (returnVal == null) ? default(T) : (T)returnVal;
		}
	}
}
