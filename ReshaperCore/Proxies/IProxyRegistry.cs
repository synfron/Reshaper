using System.Collections.ObjectModel;

namespace ReshaperCore.Proxies
{
	public interface IProxyRegistry
	{
		ObservableCollection<ProxyInfo> Proxies { get; }

		void Add(ProxyInfo proxy);
		void Init();
		void LoadProxies();
		void Remove(string name);
		void SaveProxies();
	}
}