using System;
using System.ComponentModel.Composition.Hosting;
using ReshaperCore.Providers;

namespace ReshaperCore
{
	public class Bootstrapper : IAssemblyLifetimeManager
	{
		private readonly CompositionContainerProvider _compositionContainerProvider;
		private readonly TextRulesRegistryProvider _textRulesRegistryProvider;
		private readonly HttpRulesRegistryProvider _httpRulesRegistryProvider;
		private readonly ProxyRegistryProvider _proxyRegistryProvider;
		private readonly SelfProvider _selfProvider;
		private readonly SystemProxySettingsProvider _systemProxySettingsProvider;

		public Bootstrapper()
		{
			_selfProvider = new SelfProvider();
			_textRulesRegistryProvider = new TextRulesRegistryProvider();
			_httpRulesRegistryProvider = new HttpRulesRegistryProvider();
			_proxyRegistryProvider = new ProxyRegistryProvider();
			_compositionContainerProvider = new CompositionContainerProvider();
			_systemProxySettingsProvider = new SystemProxySettingsProvider();
		}

		public void Init()
		{
			CompositionContainer compositionContainer = _compositionContainerProvider.GetInstance();

			_selfProvider.GetInstance().Init();
			_textRulesRegistryProvider.GetInstance().Init();
			_httpRulesRegistryProvider.GetInstance().Init();
			_proxyRegistryProvider.GetInstance().Init();

			foreach (Lazy<IAssemblyLifetimeManager> manager in compositionContainer.GetExports<IAssemblyLifetimeManager>())
			{
				manager.Value.Init();
			}
		}

		public void Shutdown()
		{
			foreach (Lazy<IAssemblyLifetimeManager> manager in _compositionContainerProvider.GetInstance().GetExports<IAssemblyLifetimeManager>())
			{
				manager.Value.Shutdown();
			}

			_systemProxySettingsProvider.GetInstance().ForceReset();
			_proxyRegistryProvider.GetInstance().SaveProxies();
			_textRulesRegistryProvider.GetInstance().SaveRules();
			_httpRulesRegistryProvider.GetInstance().SaveRules();
			_selfProvider.GetInstance().Shutdown();
		}
	}
}
