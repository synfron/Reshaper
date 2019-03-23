using ReshaperCore.Providers;
using ReshaperScript.Core;

namespace ReshaperScript.Providers
{
	public class ScriptEnginePoolProvider : SingletonProvider<IScriptEnginePool>
	{
		protected override IScriptEnginePool CreateInstance()
		{
			return new ScriptEnginePool();
		}
	}
}
