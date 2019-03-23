using System.ComponentModel.Composition;
using ReshaperCore;
using ReshaperScript.Providers;

namespace ReshaperScript
{
	[Export(typeof(IAssemblyLifetimeManager))]
	public class ScriptsLifetimeManager : IAssemblyLifetimeManager
	{
		private readonly ScriptRegistryProvider _scriptRegistryProvider;

		public ScriptsLifetimeManager()
		{
			_scriptRegistryProvider = new ScriptRegistryProvider();
		}

		public void Init()
		{
			_scriptRegistryProvider.GetInstance().Load();
		}

		public void Shutdown()
		{
			_scriptRegistryProvider.GetInstance().Save();
		}
	}
}
