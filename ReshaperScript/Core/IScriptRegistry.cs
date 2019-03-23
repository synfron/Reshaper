using System.Collections.Generic;

namespace ReshaperScript.Core
{
	public delegate void ScriptsListChangedHandler();

	public interface IScriptRegistry
	{
		IList<Script> Scripts { get; }

		event ScriptsListChangedHandler ScriptsListChanged;

		void AddScript(Script script);
		void DeleteScript(Script script);
		IReadOnlyList<Script> GetScripts();
		void Load();
		void Save();
	}
}