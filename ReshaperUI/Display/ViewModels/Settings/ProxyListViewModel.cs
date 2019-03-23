using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperCore.Providers;
using ReshaperCore.Proxies;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class ProxyListViewModel : ObservableViewModel
	{
		private readonly ObservableCollection<ProxyViewModel> _proxies = new ObservableCollection<ProxyViewModel>();
		private ProxyViewModel _selectedProxy;
		private readonly IProxyRegistry _proxyRegistry;

		public ObservableCollection<ProxyViewModel> Proxies
		{
			get
			{
				return _proxies;
			}
		}

		public ProxyViewModel SelectedProxy
		{
			get
			{
				return _selectedProxy;
			}
			set
			{
				_selectedProxy = value;
				OnPropertyChanged(nameof(SelectedProxy));
			}
		}

		public ProxyListViewModel()
		{
			ProxyRegistryProvider proxyRegistryProvider = new ProxyRegistryProvider();
			_proxyRegistry = proxyRegistryProvider.GetInstance();

			UpdateProxyList();
			_proxyRegistry.Proxies.CollectionChanged += Proxies_CollectionChanged;
		}

		private void Proxies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateProxyList();
		}

		private void UpdateProxyList()
		{
			Proxies.Clear();
			Proxies.Add(new ProxyViewModel());
			foreach (ProxyInfo proxyInfo in _proxyRegistry.Proxies)
			{
				Proxies.Add(new ProxyViewModel(proxyInfo));
			}
		}
	}
}
