using ReshaperCore.Proxies;

namespace ReshaperCore.Providers
{
	public class SystemProxySettingsProvider : SingletonProvider<ISystemProxySettings>
	{
		protected override ISystemProxySettings CreateInstance()
		{
			return new SystemProxySettings();
		}
	}
}
