using MsieJavaScriptEngine;

namespace ReshaperScript.Core
{
	public interface IPooledEngine
	{
        IScriptEngineAdapter ScriptEngine
		{
			get;
		}

		int UseCount
		{
			get;
		}
	}
}
