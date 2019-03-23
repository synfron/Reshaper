using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Rules;
using ReshaperScript.Core.Functions;
using ReshaperScript.Providers;

namespace ReshaperScript.Core
{
	[Export(typeof(IScriptHandler)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ScriptHandler : IScriptHandler
	{
		private const string DefaultClosure = @"
			var returnValue = (function () {{
			{0}
			}})();
			if (returnValue != null) {{
				returnValue;
			}}";
		private readonly IScriptEnginePool _scriptEnginePool;
		private readonly IScriptRegistry _scriptRegistry;

		public ScriptHandler()
		{
			ScriptEnginePoolProvider scriptEnginePoolProvider = new ScriptEnginePoolProvider();
			_scriptEnginePool = scriptEnginePoolProvider.GetInstance();

			ScriptRegistryProvider scriptRegistryProvider = new ScriptRegistryProvider();
			_scriptRegistry = scriptRegistryProvider.GetInstance();
		}

		public string RunScript(EventInfo eventInfo, string script)
		{
			IPooledEngine pooledEngine = _scriptEnginePool.CheckoutEngine();
			RunFirstRunScript(pooledEngine);
			pooledEngine.ScriptEngine.EmbedHostObject("Event", new Event(eventInfo));
			pooledEngine.ScriptEngine.EmbedHostObject("System", new Functions.System(eventInfo));
			string response = pooledEngine.ScriptEngine.Evaluate(string.Format(DefaultClosure, script))?.ToString();
			_scriptEnginePool.CheckinEngine(pooledEngine);
			return response;
		}

		public string RunNamedScript(EventInfo eventInfo, string name)
		{
			IPooledEngine pooledEngine = _scriptEnginePool.CheckoutEngine();
			RunFirstRunScript(pooledEngine);
			string response = null;
			pooledEngine.ScriptEngine.EmbedHostObject("Event", new Event(eventInfo));
			pooledEngine.ScriptEngine.EmbedHostObject("System", new Functions.System(eventInfo));
			Script selectedScript = _scriptRegistry.Scripts.FirstOrDefault(script => script.Name == name && !script.IsStaticScript);
			if (selectedScript != null)
			{
				response = pooledEngine.ScriptEngine.Evaluate(string.Format(DefaultClosure, selectedScript.Text))?.ToString();
			}
			_scriptEnginePool.CheckinEngine(pooledEngine);
			return response;
		}

		private void RunFirstRunScript(IPooledEngine pooledEngine)
		{
			if (pooledEngine.UseCount == 1)
			{
				foreach (Script script in _scriptRegistry.Scripts.Where(script => script.IsStaticScript))
				{
					pooledEngine.ScriptEngine.Execute(script.Text);
				}
			}
		}
	}
}
