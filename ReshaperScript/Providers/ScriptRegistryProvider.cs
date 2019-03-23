using ReshaperCore.Providers;
using ReshaperScript.Core;

namespace ReshaperScript.Providers
{
	public class ScriptRegistryProvider : SingletonProvider<IScriptRegistry>
	{
		protected override IScriptRegistry CreateInstance()
		{
			return new ScriptRegistry();
		}
	}
}
