using ReshaperCore.Providers;
using ReshaperScript.Settings;

namespace ReshaperScript.Providers
{
	public class ScriptEngineSettingsProvider : SingletonProvider<IScriptEngineSettings>
	{
		protected override IScriptEngineSettings CreateInstance()
		{
			return new ScriptEngineSettings();
		}
	}
}
