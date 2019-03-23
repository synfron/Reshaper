using ReshaperCore.Proxies;

namespace ReshaperCore.Providers
{
	public class ProxyRegistryProvider : SingletonProvider<IProxyRegistry>
	{
		protected override IProxyRegistry CreateInstance()
		{
			return new ProxyRegistry();
		}
	}
}
