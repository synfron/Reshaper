using ReshaperCore.Rules;

namespace ReshaperScript.Core
{
	public interface IScriptHandler
	{
		string RunNamedScript(EventInfo eventInfo, string name);

		string RunScript(EventInfo eventInfo, string command);
	}
}
