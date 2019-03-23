using ReshaperCore;
using ReshaperCore.Rules;
using ReshaperCore.Utils;
using ReshaperCore.Vars;
using ReshaperCore.Providers;

namespace ReshaperScript.Core.Functions
{
	public class System
	{
		private readonly EventInfo _eventInfo;

        public ISelf Self { get; set; } = new SelfProvider().GetInstance();

        public System(EventInfo eventInfo)
		{
			this._eventInfo = eventInfo;
		}

		public void LogMessage(string text)
		{
			Log.LogInfo(text);
		}

		public bool RemoveGlobalVariable(string name)
		{
			return Self.Variables.Remove(name);
		}

		public bool RemoveConnectionVariable(string name)
		{
			return _eventInfo.Variables.Remove(name);
		}

		public string GetGlobalVariable(string name)
		{
			IVariable<string> value = Self.Variables.GetOrDefault<string>(name);
			return value?.Value;
		}

		public string GetConnectionVariable(string name)
		{
			IVariable<string> value = _eventInfo.Variables.GetOrDefault<string>(name);
			return value?.Value;
		}

		public void SetConnectionVariable(string name, string value)
		{
			IVariable<string> variable = _eventInfo.Variables.GetOrDefault<string>(name);
			if (variable == null)
			{
				variable = _eventInfo.Variables.Add<string>(name);
			}
			variable.Value = value;
		}

		public void SetGlobalVariable(string name, string value)
		{
			IVariable<string> variable = Self.Variables.GetOrDefault<string>(name);
			if (variable == null)
			{
				variable = Self.Variables.Add<string>(name);
			}
			variable.Value = value;
		}
	}
}